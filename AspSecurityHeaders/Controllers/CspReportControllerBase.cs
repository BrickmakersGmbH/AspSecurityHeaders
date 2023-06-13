using System.ComponentModel.DataAnnotations;
using Brickmakers.AspSecurityHeaders.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brickmakers.AspSecurityHeaders.Controllers;

/// <summary>
///     A ready-to-use controller to handle CSP reports as sent by the browsers. <br />
///     The abstract controller can be implemented in your project by overriding <see cref="HandleCspReport" />. The
///     controller itself takes care of handling the JSON data and the response.<br />
///     <b>Important:</b> To use the controller, you have to extend it and then add attributes to make it a controller. It
///     should be an anonymous api controller.
///     For example:
///     <code>
///         [ApiController]
///         [Route("[controller]")]
///         [AllowAnonymous]
///         public class CspReportController : CspReportControllerBase
///         {
///             protected override Task HandleCspReport(CspReport cspReport)
///             {
///                 ...
///             }
///         }
///     </code>
///     See
///     <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy">Content-Security-Policy</a>
/// </summary>
/// <seealso cref="BmSecurityControllers.AddCspMediaType">IMvcBuilder.AddCspMediaType()</seealso>
/// <remarks>
///     To use the controller, you must also register the CSP-Media type. To do so, you can simply use
///     <see cref="BmSecurityControllers.AddCspMediaType">IMvcBuilder.AddCspMediaType()</see>
/// </remarks>
public abstract class CspReportControllerBase : ControllerBase
{
    /// <summary>
    ///     The Endpoint for CSP reports. Is mapped to the controller path and allows <c>POST</c> requests, as specified in
    ///     <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy/report-uri">report-uri</a>
    ///     . The <c>Content-Type</c> must be correctly set as well.<br />
    ///     The method must be implemented to handle CSP reports, but the controller itself takes care of providing the
    ///     endpoint and transforming the JSON data.
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
    public async Task<IActionResult> CspReport([FromBody] [Required] CspReportRequest cspReportRequest)
    {
        await HandleCspReport(cspReportRequest.CspReport);
        return NoContent();
    }

    /// <summary>
    ///     This method is called by the <see cref="CspReport" /> endpoint to "handle" a CSP report. Typically, you would
    ///     just log the incidence.
    /// </summary>
    /// <param name="cspReport">The deserialized report as sent by the browser</param>
    /// <returns>A task when the asynchronous processing of the report is done.</returns>
    protected abstract Task HandleCspReport(CspReport cspReport);
}