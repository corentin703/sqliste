using System.Text.RegularExpressions;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Sql;

namespace Sqliste.Infrastructure.Services;

internal class ProcedureResolver : IProcedureResolver
{
    private readonly IIntrospectionService _introspectionService;

    public ProcedureResolver(IIntrospectionService introspectionService)
    {
        _introspectionService = introspectionService;
    }

    public async Task<ProcedureModel?> ResolveProcedureAsync(string path, HttpMethod method, CancellationToken cancellationToken = default)
    {
        DatabaseIntrospectionModel introspection = await _introspectionService.IntrospectAsync(cancellationToken);

        ProcedureModel? procedure = introspection.Endpoints
            .FirstOrDefault(procedure => IsMatchingRoute(path, procedure) && IsMatchingVerb(method, procedure));

        return procedure;
    }
    
    private bool IsMatchingRoute(string path, ProcedureModel procedure)
    {
        string routePattern = procedure.RoutePattern
            .Replace("{?", "{")
            .Replace("?}", "}")
        ;
        
        return Regex.IsMatch(path, routePattern);
    }

    private bool IsMatchingVerb(HttpMethod method, ProcedureModel procedure)
    {
        return procedure.Operations.Any(operation => operation.Method == method);
    }
}