using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sqliste.Core.Contracts.Services;

namespace Sqliste.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProcsController : ControllerBase
{
    private readonly IDatabaseIntrospectionService _databaseIntrospectionService;

    public ProcsController(IDatabaseIntrospectionService databaseIntrospectionService)
    {
        _databaseIntrospectionService = databaseIntrospectionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        return Ok(await _databaseIntrospectionService.IntrospectAsync(cancellationToken));
    }
}