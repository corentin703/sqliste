using Microsoft.AspNetCore.Mvc;
using Sqliste.Core.Contracts.Services;

namespace Sqliste.Server.Controllers;

[Route("api/sqliste/[controller]")]
[ApiController]
public class IntrospectionController : ControllerBase
{
    private readonly IIntrospectionService _introspectionService;
    private readonly IOpenApiGenerator _openApiGenerator;

    public IntrospectionController(IIntrospectionService introspectionService, IOpenApiGenerator openApiGenerator)
    {
        _introspectionService = introspectionService;
        _openApiGenerator = openApiGenerator;
    }

    // [HttpGet]
    // public async Task<IActionResult> GetAllAsync(string d, CancellationToken cancellationToken)
    // {
    //     return Ok(await _sqlisteIntrospectionService.IntrospectAsync(cancellationToken));
    // }

    [HttpGet("swagger.json")]
    public async Task<IActionResult> GetOpenApiJson(CancellationToken cancellationToken)
    {
        return Ok(await _openApiGenerator.GenerateOpenApiJsonAsync(cancellationToken));
    }
    
    // [HttpDelete]
    // public IActionResult Delete()
    // {
    //     _sqlisteIntrospectionService.Clear();
    //     return NoContent();
    // }
}