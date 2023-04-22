using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Sql;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Sqliste.Core.Utils.Uri;

namespace Sqliste.Core.Services;

public class RequestHandlerService : IRequestHandlerService
{
    private readonly IDatabaseIntrospectionService _databaseIntrospectionService;
    private readonly IDatabaseService _databaseService;

    public RequestHandlerService(IDatabaseIntrospectionService databaseIntrospectionService, IDatabaseService databaseService)
    {
        _databaseIntrospectionService = databaseIntrospectionService;
        _databaseService = databaseService;
    }

    public async Task<HttpResponseModel> HandleRequestAsync(HttpRequestModel request, CancellationToken cancellationToken = default)
    {
        List<ProcedureModel> routes = await _databaseIntrospectionService.IntrospectAsync(cancellationToken);

        ProcedureModel? procedure = routes
            .FirstOrDefault(route => IsMatchingRoute(request, route) && IsMatchingVerb(request, route));

        if (procedure == null)
        {
            return new HttpResponseModel()
            {
                Status = HttpStatusCode.NotFound,
            };
        }

        Dictionary<string, object?> sqlParams = GetParams(request, procedure);

        List<IDictionary<string, object>>? result = 
            await _databaseService.QueryAsync($"EXEC {procedure.Schema}.{procedure.Name}", sqlParams, cancellationToken);

        return new HttpResponseModel()
        {
            Data = result,
            Status = HttpStatusCode.OK,
        };
    }

    private Dictionary<string, object?> GetParams(HttpRequestModel request, ProcedureModel procedure)
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

        return queryParams;
    }

    private void AddParams(
        Dictionary<string, object?> paramsBag, 
        List<ArgumentModel> procedureArgs, 
        string paramName,
        object? paramValue
    )
    {
        if (procedureArgs.Any(argument => argument.Name == paramName)) 
            paramsBag.TryAdd(paramName, paramValue);
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
        Dictionary<string, string> routeParams = new();

        Match paramsMatch = Regex.Match(uri, procedure.RoutePattern);

        if (!paramsMatch.Success) 
            return routeParams;

        procedure.RouteParamNames.ForEach(paramName =>
        {
            string paramValue = paramsMatch.Groups[paramName].Value;
            if (string.IsNullOrEmpty(paramValue))
                return;

            routeParams.Add(paramName, paramValue);
        });

        return routeParams;
    }
}