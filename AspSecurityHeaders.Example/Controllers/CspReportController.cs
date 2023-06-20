using System.Threading.Tasks;
using Brickmakers.AspSecurityHeaders.Controllers;
using Brickmakers.AspSecurityHeaders.Controllers.Models;
using Brickmakers.AspSecurityHeaders.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brickmakers.AspSecurityHeaders.Example.Controllers;

[ApiController]
[Route("[controller]")]
public class CspReportController : CspReportControllerBase
{
    private readonly ILogger<CspReportController> _logger;

    public CspReportController(ILogger<CspReportController> logger)
    {
        _logger = logger;
    }

    protected override Task HandleCspReport(CspReport cspReport)
    {
        _logger.LogCspReport(cspReport);
        return Task.CompletedTask;
    }
}