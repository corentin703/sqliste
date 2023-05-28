using System.Net;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Constants;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Pipeline;
using Sqliste.Core.Models.Sql;
using Sqliste.Database.Common.Contracts.Services;

namespace Sqliste.Infrastructure.Services;

internal class RequestHandler : IRequestHandler
{
    private readonly IIntrospectionService _introspectionService;
    private readonly ILogger<RequestHandler> _logger;
    private readonly IDatabaseSessionAccessor _sessionAccessor;
    private readonly IDatabaseGateway _databaseGateway;
    private readonly IParametersResolver _parametersResolver;
    
    public RequestHandler(
        IIntrospectionService introspectionService,
        ILogger<RequestHandler> logger, 
        IDatabaseSessionAccessor sessionAccessor, 
        IDatabaseGateway databaseGateway, 
        IParametersResolver parametersResolver
    )
    {
        _introspectionService = introspectionService;
        _logger = logger;
        _sessionAccessor = sessionAccessor;
        _databaseGateway = databaseGateway;
        _parametersResolver = parametersResolver;
    }

    public async Task<PipelineBag> HandleRequestAsync(PipelineBag pipeline, CancellationToken cancellationToken = default)
    {
        DatabaseIntrospectionModel introspection = await _introspectionService.IntrospectAsync(cancellationToken);

        if (pipeline.Procedure == null)
        {
            _logger.LogInformation("Matching procedure not found for path {Path}", pipeline.Request.Path);
            pipeline.Response = new PipelineResponseBag()
            {
                Status = HttpStatusCode.NotFound,
            };
            
            return pipeline;
        }

        ProcedureModel procedure = pipeline.Procedure;
        
        _logger.LogDebug("Procedure found for path {Path} : {ProcedureName}", pipeline.Request.Path, procedure.Name);

        await ExecMiddlewaresAsync(pipeline, introspection.BeforeMiddlewares, false, cancellationToken: cancellationToken);
        if (!pipeline.Response.Next)
            return pipeline;

        if (pipeline.Response.Error == null)
            pipeline = await ExecRequestAsync(pipeline, procedure, cancellationToken) ?? pipeline;
        
        await ExecMiddlewaresAsync(pipeline, introspection.AfterMiddlewares, true, cancellationToken: cancellationToken);

        if (pipeline.Response.Session != null)
            await _sessionAccessor.SetSessionAsync(pipeline.Response.Session, cancellationToken);
        
        pipeline.Response.ContentType ??= procedure.ContentType;
        return pipeline;
    }

    private async Task ExecMiddlewaresAsync(
        PipelineBag pipeline, 
        List<ProcedureModel> middlewares,
        bool handleErrors,
        CancellationToken cancellationToken = default
    )
    {
        List<ProcedureModel> middlewaresToRun = middlewares
            .Where(middleware => pipeline.Request.Path.StartsWith(middleware.Route))
            .ToList();

        foreach (ProcedureModel middleware in middlewaresToRun)
        {
            if (pipeline.Response.Error != null && handleErrors)
            {
                bool isErrorHandler = middleware.Arguments.Exists(arg =>
                    arg.Name is SystemQueryParametersConstants.Error or SystemQueryParametersConstants.Error);

                if (!isErrorHandler) 
                    continue;
            }

            Dictionary<string, object?> sqlParams = await _parametersResolver.GetParamsAsync(pipeline, middleware, cancellationToken);
            PipelineResponseBag middlewareResponse = await _databaseGateway.ExecProcedureAsync(pipeline.Request, middleware, sqlParams, cancellationToken);

            pipeline.Response = MergeResponses(pipeline.Response,  middlewareResponse);

            if (!middlewareResponse.Next || (middlewareResponse.Error != null && !handleErrors))
                break;
        }

        if (handleErrors && pipeline.Response.Error?.RawException != null)
        {
            pipeline.Response.Status = HttpStatusCode.InternalServerError;
            pipeline.Response.Body = JsonSerializer.Serialize(new
            {
                Message = "An unhandled error occurred",
            });
        }
    }

    private async Task<PipelineBag?> ExecRequestAsync(
        PipelineBag pipeline, 
        ProcedureModel procedure,
        CancellationToken cancellationToken = default
    )
    {
        Dictionary<string, object?> sqlParams = await _parametersResolver.GetParamsAsync(pipeline, procedure, cancellationToken);
        PipelineResponseBag response = await _databaseGateway.ExecProcedureAsync(pipeline.Request, procedure, sqlParams, cancellationToken);
        pipeline.Response = MergeResponses(pipeline.Response, response);

        return pipeline;
    }

    private PipelineResponseBag MergeResponses(PipelineResponseBag originalResponse, PipelineResponseBag? updatedResponse)
    {
        if (updatedResponse == null) 
            return originalResponse;

        PipelineResponseBag mergedResponse = new();

        PropertyInfo[] properties = typeof(PipelineResponseBag).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            if (!property.CanRead || !property.CanWrite)
                continue;

            object? originalPropertyValue = property.GetValue(originalResponse);
            object? updatedPropertyValue = property.GetValue(updatedResponse);
            
            if (updatedPropertyValue == null)
                property.SetValue(mergedResponse, originalPropertyValue);
            else if (originalPropertyValue == null || !originalPropertyValue.Equals(updatedPropertyValue))
                property.SetValue(mergedResponse, updatedPropertyValue);
        }

        if (updatedResponse.Error == null)
            mergedResponse.Error = null;

        return mergedResponse;
    }
}