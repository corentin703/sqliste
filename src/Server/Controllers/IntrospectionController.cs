using Microsoft.AspNetCore.Mvc;
using Sqliste.Core.Contracts.Services;

namespace Sqliste.Server.Controllers;

[Route("api/sqliste/[controller]")]
[ApiController]
public class IntrospectionController : ControllerBase
{
    private readonly IDatabaseIntrospectionService _databaseIntrospectionService;
    private readonly IDatabaseOpenApiService _databaseOpenApiService;

    public IntrospectionController(IDatabaseIntrospectionService databaseIntrospectionService, IDatabaseOpenApiService databaseOpenApiService)
    {
        _databaseIntrospectionService = databaseIntrospectionService;
        _databaseOpenApiService = databaseOpenApiService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        return Ok(await _databaseIntrospectionService.IntrospectAsync(cancellationToken));
    }

    [HttpGet("swagger.json")]
    public async Task<IActionResult> GetOpenApiJson(CancellationToken cancellationToken)
    {
        return Ok(await _databaseOpenApiService.GenerateOpenApiJsonAsync(cancellationToken));
    }

    [HttpDelete]
    public IActionResult Delete()
    {
        _databaseIntrospectionService.Clear();
        return NoContent();
    }
}