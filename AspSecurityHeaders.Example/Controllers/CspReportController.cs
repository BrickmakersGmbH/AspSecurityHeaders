using System.Threading.Tasks;
using Brickmakers.AspSecurityHeaders.Controllers;
using Brickmakers.AspSecurityHeaders.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brickmakers.AspSecurityHeaders.Example.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class CspReportController : CspReportControllerBase
{
    private readonly ILogger<CspReportController> _logger;

    public CspReportController(ILogger<CspReportController> logger)
    {
        _logger = logger;
    }

    protected override Task HandleCspReport(CspReport cspReport)
    {
        _logger.LogError("{}", cspReport.ToString());
        return Task.CompletedTask;
    }
}