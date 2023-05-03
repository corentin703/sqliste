using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models;
using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.SqlAnnotations.OpenApi;
using System.Text.RegularExpressions;

namespace Sqliste.Core.Services;

public abstract class DatabaseOpenApiService : IDatabaseOpenApiService
{
    private readonly ILogger<DatabaseOpenApiService> _logger;
    private readonly IDatabaseIntrospectionService _databaseIntrospectionService;

    private const string ResourceRouteRegexPattern = @"^(\/api)?\/(?<resource>\w+).*$";

    protected DatabaseOpenApiService(
        ILogger<DatabaseOpenApiService> logger, 
        IDatabaseIntrospectionService databaseIntrospectionService
    )
    {
        _logger = logger;
        _databaseIntrospectionService = databaseIntrospectionService;
    }

    public async Task<string> GenerateOpenApiJsonAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Generating OpenApiDocument");
        DatabaseIntrospectionModel introspection = await _databaseIntrospectionService.IntrospectAsync(cancellationToken);

        OpenApiPaths paths = new();
        foreach (ProcedureModel procedure in introspection.Endpoints)
        {
            OpenApiPathItem? path;
            if (!paths.TryGetValue(procedure.Route, out path))
            {
                path = new OpenApiPathItem()
                {
                    Operations = new Dictionary<OperationType, OpenApiOperation>(),
                };

                paths.Add(procedure.Route, path);
            }

            foreach (HttpOperationModel operationModel in procedure.Operations)
            {
                OperationType operationType = HttpMethodToOperationType(operationModel.Method);
                OpenApiOperation operation = await GenerateOperationAsync(procedure, cancellationToken);
                operation.OperationId = operationModel.Id;

                path.Operations.TryAdd(operationType, operation);
            }
        }

        OpenApiDocument openApiDocument = await GetDocumentFromDatabaseAsync(cancellationToken);
        openApiDocument.Paths = paths;

        _logger.LogDebug("OpenApi document generated");
        string openApiJson = openApiDocument.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0);
        _logger.LogDebug("OpenApi document serialized as v3");

        return openApiJson;
    }

    protected abstract Task<OpenApiDocument> GetDocumentFromDatabaseAsync(CancellationToken cancellationToken);

    protected abstract Task<OpenApiTypeGetResponseModel> GetOpenApiTypeFromSqlTypeAsync(string sqlType, CancellationToken cancellationToken);

    private async Task<OpenApiOperation> GenerateOperationAsync(ProcedureModel procedure, CancellationToken cancellationToken)
    {
        OpenApiResponses responses = GenerateResponses(procedure);
        List<OpenApiParameter> parameters = new();

        foreach (ProcedureArgumentModel argument in procedure.Arguments)
        {
            if (argument.IsSystemParam)
                continue;

            OpenApiTypeGetResponseModel openApiTypeInfo = 
                await GetOpenApiTypeFromSqlTypeAsync(argument.SqlDataType, cancellationToken);

            parameters.Add(new OpenApiParameter()
            {
                Name = argument.Name,
                Required = procedure.RoutePattern.Contains(argument.Name),
                Schema = new OpenApiSchema()
                {
                    Type = openApiTypeInfo.Type,
                    Format = openApiTypeInfo.Format,
                },
                In = argument.Location,
            });
        }

        TakesSqlAnnotation? takesSqlAnnotation = procedure.Annotations
            .FirstOrDefault(annotation => annotation is TakesSqlAnnotation) as TakesSqlAnnotation;

        List<AcceptsSqlAnnotation> acceptAnnotations = procedure.Annotations
            .Where(annotation => annotation is AcceptsSqlAnnotation)
            .Cast<AcceptsSqlAnnotation>()
            .ToList();

        OpenApiOperation openApiOperation = new()
        {
            Tags = GenerateTags(procedure.Route),
            Parameters = parameters,
            Responses = responses,
        };

        if (takesSqlAnnotation == null) 
            return openApiOperation;

        OpenApiRequestBody requestBody = new()
        {
            Required = takesSqlAnnotation.Required,
        };

        if (!string.IsNullOrEmpty(takesSqlAnnotation.Type))
        {
            Dictionary<string, OpenApiMediaType> content = new();

            acceptAnnotations.ForEach(accepts =>
            {
                content.Add(accepts.Mime, new OpenApiMediaType()
                {
                    Schema = new OpenApiSchema()
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = takesSqlAnnotation.Type,
                            Type = ReferenceType.Schema,
                        },
                    },
                });
            });

            requestBody.Content = content;
        }

        if (!string.IsNullOrEmpty(takesSqlAnnotation.Description))
            requestBody.Description = takesSqlAnnotation.Description;

        openApiOperation.RequestBody = requestBody;
        return openApiOperation;
    }

    private OpenApiResponses GenerateResponses(ProcedureModel procedure)
    {
        List<RespondsSqlAnnotation> respondAnnotations = procedure.Annotations
            .Where(annotation => annotation is RespondsSqlAnnotation)
            .Cast<RespondsSqlAnnotation>()
            .ToList();

        List<ProducesSqlAnnotation> produceAnnotations = procedure.Annotations
            .Where(annotation => annotation is ProducesSqlAnnotation)
            .Cast<ProducesSqlAnnotation>()
            .ToList();

        OpenApiResponses responses = new();
        respondAnnotations.ForEach(responds =>
        {
            OpenApiResponse response = new();

            if (!string.IsNullOrEmpty(responds.Description))
                response.Description = responds.Description;

            if (!string.IsNullOrEmpty(responds.Type))
            {
                Dictionary<string, OpenApiMediaType> content = new();

                produceAnnotations.ForEach(produces =>
                {
                    content.Add(produces.Mime, new OpenApiMediaType()
                    {
                        Schema = new OpenApiSchema()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = responds.Type,
                                Type = ReferenceType.Schema,
                            },
                        },
                    });
                });

                response.Content = content;
            }

            responses.Add(((int)responds.Status).ToString(), response);
        });

        return responses;
    }

    private List<OpenApiTag> GenerateTags(string route)
    {
        string resourceName = ExtractResource(route);

        List<OpenApiTag> tags = new()
        {
            new OpenApiTag()
            {
                Name = resourceName,
            }
        };

        return tags;
    }

    private OperationType HttpMethodToOperationType(HttpMethod httpMethod)
    {
        if (httpMethod == HttpMethod.Get)
            return OperationType.Get;

        if (httpMethod == HttpMethod.Post)
            return OperationType.Post;

        if (httpMethod == HttpMethod.Put)
            return OperationType.Put;

        if (httpMethod == HttpMethod.Patch)
            return OperationType.Patch;

        if (httpMethod == HttpMethod.Delete)
            return OperationType.Delete;

        return OperationType.Get;
    }

    private string ExtractResource(string route)
    {
        Match match = Regex.Match(route, ResourceRouteRegexPattern);

        if (!match.Success)
            return "Default";

        return match.Groups["resource"].Value;
    }
}