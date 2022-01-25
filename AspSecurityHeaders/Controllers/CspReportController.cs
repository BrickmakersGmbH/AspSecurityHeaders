using System.ComponentModel.DataAnnotations;
using Brickmakers.AspSecurityHeaders.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brickmakers.AspSecurityHeaders.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
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
    public IActionResult CspReport([FromBody] [Required] CspReportRequest cspReportRequest)
    {
        _logger.LogError("{}", cspReportRequest.CspReport);
        return NoContent();
    }
}