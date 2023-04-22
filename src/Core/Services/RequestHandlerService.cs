﻿using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.Utils.Uri;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Sqliste.Core.Services;

public abstract class RequestHandlerService : IRequestHandlerService
{
    protected readonly IDatabaseIntrospectionService DatabaseIntrospectionService;
    protected readonly IDatabaseService DatabaseService;
    private readonly ILogger<RequestHandlerService> _logger;

    protected RequestHandlerService(IDatabaseIntrospectionService databaseIntrospectionService, IDatabaseService databaseService, ILogger<RequestHandlerService> logger)
    {
        DatabaseIntrospectionService = databaseIntrospectionService;
        DatabaseService = databaseService;
        _logger = logger;
    }

    public async Task<HttpResponseModel> HandleRequestAsync(HttpRequestModel request, CancellationToken cancellationToken = default)
    {
        List<ProcedureModel> routes = await DatabaseIntrospectionService.IntrospectAsync(cancellationToken);

        ProcedureModel? procedure = routes
            .FirstOrDefault(route => IsMatchingRoute(request, route) && IsMatchingVerb(request, route));

        if (procedure == null)
        {
            _logger.LogInformation("Matching procedure not found for path {path}", request.Path);
            return new HttpResponseModel()
            {
                Status = HttpStatusCode.NotFound,
            };
        }

        _logger.LogDebug("Procedure found for path {path} : {procedureName}", request.Path, procedure.Name);
        Dictionary<string, object?> sqlParams = GetParams(request, procedure);
        return await ExecRequestAsync(procedure, sqlParams, cancellationToken);
    }

    protected abstract Task<HttpResponseModel> ExecRequestAsync(ProcedureModel procedure, Dictionary<string, object?> sqlParams, CancellationToken cancellationToken);

    protected virtual Dictionary<string, object?> GetParams(HttpRequestModel request, ProcedureModel procedure)
    {
        Dictionary<string, object?> queryParams = new Dictionary<string, object?>();
        Dictionary<string, string> uriParams = ParseUriParams(request.Path, procedure);

        foreach (KeyValuePair<string, string> uriParam in uriParams)
        {
            AddParams(queryParams, procedure.Arguments, uriParam.Key, uriParam.Value);
        }

        AddParams(queryParams, procedure.Arguments, "body", request.Body);
        AddParams(queryParams, procedure.Arguments, "cookies", JsonSerializer.Serialize(request.Cookies));
        AddParams(queryParams, procedure.Arguments, "headers", JsonSerializer.Serialize(request.Headers));

        _logger.LogDebug("Added {paramCount} for {procedureName}", queryParams.Count, procedure.Name);
        return queryParams;
    }

    private void AddParams(
        Dictionary<string, object?> paramsBag, 
        List<ArgumentModel> procedureArgs, 
        string paramName,
        object? paramValue
    )
    {
        // ReSharper disable once SimplifyLinqExpressionUseAll
        if (!procedureArgs.Any(argument => argument.Name == paramName)) 
            _logger.LogInformation("Ignoring param {paramName}", paramName);

        paramsBag.TryAdd(paramName, paramValue);
        _logger.LogDebug("Param {paramName} added", paramName);
    }

    private bool IsMatchingRoute(HttpRequestModel request, ProcedureModel procedure)
    {
        return Regex.IsMatch(UriPathParser.ExtractUriPath(request.Path), procedure.RoutePattern);
    }

    private bool IsMatchingVerb(HttpRequestModel request, ProcedureModel procedure)
    {
        return procedure.HttpMethods.Any(method => method == request.Method);
    }

    private Dictionary<string, string> ParseUriParams(string uri, ProcedureModel procedure)
    {
        Dictionary<string, string> pathParams = new();

        Match paramsMatch = Regex.Match(uri, procedure.RoutePattern);
        if (!paramsMatch.Success)
        {
            _logger.LogDebug("Path params's pattern didn't match for {procedureName} (path: {path})", procedure.Name, uri);
            return pathParams;
        }

        procedure.RouteParamNames.ForEach(paramName =>
        {
            string paramValue = paramsMatch.Groups[paramName].Value;
            if (string.IsNullOrEmpty(paramValue))
                return;

            pathParams.Add(paramName, paramValue);
        });

        _logger.LogDebug("Found {paramCount} path params for {procedureName} (path: {path})", pathParams.Count, procedure.Name, uri);
        return pathParams;
    }
}