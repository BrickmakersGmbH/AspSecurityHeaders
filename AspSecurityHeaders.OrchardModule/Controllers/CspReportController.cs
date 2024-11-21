using System.ComponentModel.DataAnnotations;
using Brickmakers.AspSecurityHeaders.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brickmakers.AspSecurityHeaders.OrchardModule.Controllers;

/// <summary>
///     A controller to handle CSP reports as sent by the browsers. <br />
///     The controller itself takes care of handling CSP reports by logging the with a standard ILogger.<br />
///     See
///     <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy">Content-Security-Policy</a>
/// </summary>
[ApiController]
[Route("[controller]")]
public class CspReportController : ControllerBase
{
    private readonly ILogger<CspReportController> _logger;

    /// <summary>
    ///     Default constructor
    /// </summary>
    /// <param name="logger">The logger to log the reports to</param>
    public CspReportController(ILogger<CspReportController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     The Endpoint for CSP reports. Is mapped to the controller path and allows <c>POST</c> requests, as specified in
    ///     <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy/report-uri">report-uri</a>
    ///     . The <c>Content-Type</c> must be correctly set as well.<br /> It uses the given <see cref="_logger" /> to log CSP
    ///     reports as error message.
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
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public Task<IActionResult> CspReport([FromBody] [Required] CspReportRequest cspReportRequest)
    {
        using var scope = _logger.BeginScope(cspReportRequest.CspReport.ToDictionary());
        _logger.LogError("{CspReport}", cspReportRequest.CspReport);
        return Task.FromResult<IActionResult>(NoContent());
    }
}