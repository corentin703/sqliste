using System.Net;
using System.Text.RegularExpressions;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Requests;
using Sqliste.Core.Models.Response;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.SqlAnnotations;

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

        ProcedureModel? procedure = routes.FirstOrDefault(route => IsMatchingRoute(request, route));

        if (procedure == null)
            throw new NotImplementedException();

        var result = await _databaseService.QueryAsync($"EXEC {procedure.Schema}.{procedure.Name}", new { }, cancellationToken);

        return new HttpResponseModel()
        {
            Data = result,
            Status = HttpStatusCode.OK,
        };
    }

    private bool IsMatchingRoute(HttpRequestModel request, ProcedureModel procedure)
    {
        //RouteSqlAnnotation? routeAnnotation = procedure.Annotations
        //    .FirstOrDefault(annotation => annotation is RouteSqlAnnotation) as RouteSqlAnnotation;

        //if (routeAnnotation == null) 
        //    return false; // TODO : Route must be optional (default to procedure naming based route)


        return Regex.IsMatch(request.Path, procedure.RoutePattern);
    }
}