using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.SqlAnnotations.OpenApi;

namespace Sqliste.Core.Services;

public class DatabaseOpenApiService : IDatabaseOpenApiService
{
    private readonly ILogger<DatabaseOpenApiService> _logger;
    private readonly IDatabaseIntrospectionService _databaseIntrospectionService;

    private const string ResourceRouteRegexPattern = @"^(\/api)?\/(?<resource>\w+).*$";

    public DatabaseOpenApiService(ILogger<DatabaseOpenApiService> logger, IDatabaseIntrospectionService databaseIntrospectionService)
    {
        _logger = logger;
        _databaseIntrospectionService = databaseIntrospectionService;
    }

    public async Task<string> GenerateOpenApiJsonAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Generating OpenApiDocument");
        List<ProcedureModel> introspection = await _databaseIntrospectionService.IntrospectAsync(cancellationToken);

        OpenApiPaths paths = new();
        introspection.ForEach(procedure =>
        {
            OpenApiPathItem path;
            if (!paths.TryGetValue(procedure.Route, out path))
            {
                path = new OpenApiPathItem()
                {
                    Operations = new Dictionary<OperationType, OpenApiOperation>(),
                };

                paths.Add(procedure.Route, path);
            }

            List<OperationType> operationTypes = procedure.HttpMethods.Select(HttpMethodToOperationType).ToList();
            operationTypes.ForEach(operationType => 
                path.Operations.TryAdd(operationType, GenerateOperation(procedure))
            );
        });

        OpenApiDocument openApiDocument = new()
        {
            
            Info = new OpenApiInfo()
            {
                Version = "1.0.0",
                Title = "SQListe",
            },
            Paths = paths,
        };

        _logger.LogDebug("OpenApi document generated");
        string openApiJson = openApiDocument.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json);
        _logger.LogDebug("OpenApi document serialized as v3");

        return openApiJson;
    }

    private OpenApiOperation GenerateOperation(ProcedureModel procedure)
    {
        List<OpenApiTag> tags = GenerateTags(procedure.Route);
        OpenApiResponses responses = GenerateResponses(procedure);

        return new OpenApiOperation()
        {
            Tags = tags,
            Responses = responses,
        };
    }

    private OpenApiResponses GenerateResponses(ProcedureModel procedure)
    {
        List<ProducesResponseTypeSqlAnnotation> producesResponseAnnotations = procedure.Annotations
            .Where(annotation => annotation is ProducesResponseTypeSqlAnnotation)
            .Cast<ProducesResponseTypeSqlAnnotation>()
            .ToList();

        OpenApiResponses responses = new OpenApiResponses();
        producesResponseAnnotations.ForEach(annotation =>
        {
            responses.Add(((int)annotation.StatusCode).ToString(), new OpenApiResponse()
            {
                Description = annotation.Description,
            });
        });

        return responses;
    }

    private List<OpenApiTag> GenerateTags(string route)
    {
        string resourceName = ExtractResource(route);
        //bool isApi = IsApi(route);

        List<OpenApiTag> tags = new()
        {
            new OpenApiTag()
            {
                Name = resourceName,
            }
        };

        //if (isApi)
        //{
        //    tags.Add(new OpenApiTag()
        //    {
        //        Name = "API",
        //    });
        //}

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

    private bool IsApi(string route)
    {
        return route.StartsWith("/api");
    }

    private string ExtractResource(string route)
    {
        Match match = Regex.Match(route, ResourceRouteRegexPattern);

        if (!match.Success)
            return "Default";

        return match.Groups["resource"].Value;
    }
}