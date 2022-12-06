using System.ComponentModel.DataAnnotations;
using Brickmakers.AspSecurityHeaders.OrchardModule.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brickmakers.AspSecurityHeaders.OrchardModule.Controllers;

[ApiController]
[Route("[controller]")]
public class CspReportController : ControllerBase
{
    private readonly ILogger<CspReportController> _logger;


    public CspReportController(ILogger<CspReportController> logger)
    {
        _logger = logger;
    }


    [HttpPost]
    [RequestSizeLimit(100000)] // 100 kB
    [Consumes("application/csp-report", "application/json", "text/json")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public Task<IActionResult> CspReport([FromBody] [Required] CspReportRequest cspReportRequest)
    {
        _logger.LogCritical("{}", cspReportRequest.CspReport);
        return Task.FromResult<IActionResult>(NoContent());
    }
}