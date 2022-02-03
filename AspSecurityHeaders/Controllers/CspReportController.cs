using System.ComponentModel.DataAnnotations;
using Brickmakers.AspSecurityHeaders.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brickmakers.AspSecurityHeaders.Controllers;

/// <summary>
///     A ready-to-use controller to handle CSP reports as sent by the browsers. <br />
///     See
///     <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy">Content-Security-Policy</a>
/// </summary>
/// <seealso cref="BmSecurityControllers.AddSecurityControllers">IMvcBuilder.AddSecurityControllers()</seealso>
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

    /// <summary>
    ///     The Endpoint for CSP reports. Is mappend to <c>/CspReport</c> and allows <c>POST</c> requests, as specified in
    ///     <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy/report-uri">report-uri</a>
    ///     . The <c>Content-Type</c> must be correctly set as well.<br />
    ///     The controller itself will use
    ///     <see cref="LoggerExtensions.LogError(Microsoft.Extensions.Logging.ILogger,string?,object?[])">ILogger.LogError</see>
    ///     to log the CSP report. The method can be overridden to add custom report handling.
    /// </summary>
    /// <param name="cspReportRequest">The deserialized CSP request sent by the browsers.</param>
    /// <returns>204 (No Content)</returns>
    /// <remarks>
    ///     Since this is an <see cref="AllowAnonymousAttribute">[AllowAnonymous]</see> Endpoint, it is publicly
    ///     available. To prevent misuse, the request size is currently limited to 100 kB. However, this should be enough for
    ///     any CSP report, even if containing samples.
    /// </remarks>
    [HttpPost]
    [RequestSizeLimit(100000)] // 100 kB
    [Consumes("application/csp-report", "application/json", "text/json")]
    public IActionResult CspReport([FromBody] [Required] CspReportRequest cspReportRequest)
    {
        _logger.LogError("{}", cspReportRequest.CspReport);
        return NoContent();
    }
}