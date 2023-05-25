﻿using System.Net;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Constants;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http.FormData;
using Sqliste.Core.Models.Pipeline;
using Sqliste.Core.Models.Sql;
using Sqliste.Database.Common.Contracts.Services;

namespace Sqliste.Infrastructure.Services;

internal class RequestHandler : IRequestHandler
{
    private readonly ISqlisteIntrospectionService _introspectionService;
    private readonly ILogger<RequestHandler> _logger;
    private readonly IDatabaseSessionAccessor _sessionAccessor;
    private readonly IDatabaseGateway _databaseGateway;
    
    public RequestHandler(
        ISqlisteIntrospectionService introspectionService,
        ILogger<RequestHandler> logger, IDatabaseSessionAccessor sessionAccessor, IDatabaseGateway databaseGateway)
    {
        _introspectionService = introspectionService;
        _logger = logger;
        _sessionAccessor = sessionAccessor;
        _databaseGateway = databaseGateway;
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

            Dictionary<string, object?> sqlParams = await GetParamsAsync(pipeline, middleware, cancellationToken);
            PipelineResponseBag? middlewareResponse = await _databaseGateway.ExecProcedureAsync(pipeline.Request, middleware, sqlParams, cancellationToken);

            if (middlewareResponse == null)
                continue;

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
        Dictionary<string, object?> sqlParams = await GetParamsAsync(pipeline, procedure, cancellationToken);
        PipelineResponseBag? response = await _databaseGateway.ExecProcedureAsync(pipeline.Request, procedure, sqlParams, cancellationToken);
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

    private async Task<Dictionary<string, object?>> GetParamsAsync(PipelineBag pipeline, ProcedureModel procedure, CancellationToken cancellationToken)
    {
        Dictionary<string, object?> sqlParams = new();

        #region Request

        foreach (KeyValuePair<string, string> uriParam in pipeline.Request.PathParams)
        {
            sqlParams.TryAdd(uriParam.Key, uriParam.Value);
        }

        foreach (KeyValuePair<string, string> queryParam in pipeline.Request.QueryParams)
        {
            sqlParams.TryAdd(queryParam.Key, queryParam.Value);
        }

        sqlParams.TryAdd(SystemQueryParametersConstants.RequestBody, pipeline.Request.Body);
        sqlParams.TryAdd(SystemQueryParametersConstants.RequestContentType, pipeline.Request.ContentType);
        sqlParams.TryAdd(SystemQueryParametersConstants.RequestCookies, pipeline.Request.Cookies ?? "{}");
        sqlParams.TryAdd(SystemQueryParametersConstants.RequestHeaders, pipeline.Request.Headers ?? "{}");
        sqlParams.TryAdd(SystemQueryParametersConstants.RequestPath, pipeline.Request.Path);
        sqlParams.TryAdd(SystemQueryParametersConstants.PathParams, JsonSerializer.Serialize(pipeline.Request.PathParams));
        sqlParams.TryAdd(SystemQueryParametersConstants.QueryParams, JsonSerializer.Serialize(pipeline.Request.QueryParams));
        sqlParams.TryAdd(SystemQueryParametersConstants.RequestModel, JsonSerializer.Serialize(pipeline.Request));
        AddFormDataParameters(sqlParams, pipeline);
        
        #endregion

        #region Response

        sqlParams.TryAdd(SystemQueryParametersConstants.ResponseBody, pipeline.Response.Body);
        sqlParams.TryAdd(SystemQueryParametersConstants.ResponseCookies, pipeline.Response.Cookies ?? "{}");
        sqlParams.TryAdd(SystemQueryParametersConstants.PipelineStorage, pipeline.Response.PipelineStorage);
        sqlParams.TryAdd(SystemQueryParametersConstants.ResponseHeaders, pipeline.Response.Headers ?? "{}");
        sqlParams.TryAdd(SystemQueryParametersConstants.ResponseStatus, pipeline.Response.Status);
        sqlParams.TryAdd(SystemQueryParametersConstants.ResponseContentType, pipeline.Response.ContentType ?? procedure.ContentType);
        sqlParams.TryAdd(SystemQueryParametersConstants.ResponseModel, JsonSerializer.Serialize(pipeline.Response));

        #endregion

        if (procedure.Arguments.Any(arg => arg.Name == SystemQueryParametersConstants.Session))
        {
            string? session = await _sessionAccessor.GetSessionAsync(cancellationToken);
            sqlParams.TryAdd(SystemQueryParametersConstants.Session, session);
        }

        if (pipeline.Response.Error != null)
            sqlParams.TryAdd(SystemQueryParametersConstants.Error, JsonSerializer.Serialize(pipeline.Response.Error));

        _logger.LogDebug("Added {ParamCount} for {ProcedureName}", sqlParams.Count, procedure.Name);
        return sqlParams;
    }

    private void AddFormDataParameters(Dictionary<string, object?> sqlParams, PipelineBag pipeline)
    {
        if (pipeline.Request.FormData == null) 
            return;
        
        sqlParams.TryAdd(SystemQueryParametersConstants.RequestFormData, JsonSerializer.Serialize(pipeline.Request.FormData));

        List<FormDataFile> files = pipeline.Request.FormData
            .Select(item => item.Value)
            .Where(value => value is FormDataFile).Cast<FormDataFile>().ToList();
            
        foreach (FormDataFile file in files)
        {
            sqlParams.TryAdd($"{SystemQueryParametersConstants.RequestFormFilePrefix}{file.Name}", file.Content);
        }
    }
}