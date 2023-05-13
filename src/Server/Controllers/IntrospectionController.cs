using Microsoft.AspNetCore.Mvc;
using Sqliste.Core.Contracts.Services;

namespace Sqliste.Server.Controllers;

[Route("api/sqliste/[controller]")]
[ApiController]
public class IntrospectionController : ControllerBase
{
    private readonly ISqlisteIntrospectionService _sqlisteIntrospectionService;
    private readonly ISqlisteOpenApiService _sqlisteOpenApiService;

    public IntrospectionController(ISqlisteIntrospectionService sqlisteIntrospectionService, ISqlisteOpenApiService sqlisteOpenApiService)
    {
        _sqlisteIntrospectionService = sqlisteIntrospectionService;
        _sqlisteOpenApiService = sqlisteOpenApiService;
    }

    // [HttpGet]
    // public async Task<IActionResult> GetAllAsync(string d, CancellationToken cancellationToken)
    // {
    //     return Ok(await _sqlisteIntrospectionService.IntrospectAsync(cancellationToken));
    // }

    [HttpGet("swagger.json")]
    public async Task<IActionResult> GetOpenApiJson(CancellationToken cancellationToken)
    {
        return Ok(await _sqlisteOpenApiService.GenerateOpenApiJsonAsync(cancellationToken));
    }
    
    // [HttpDelete]
    // public IActionResult Delete()
    // {
    //     _sqlisteIntrospectionService.Clear();
    //     return NoContent();
    // }
}