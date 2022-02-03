using Brickmakers.AspSecurityHeaders.Controllers;
using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.ContentSecurityPolicy;

namespace Brickmakers.AspSecurityHeaders.CspBuilderExtensions;

/// <summary>
///     Extensions to <see cref="CspBuilder" /> to add the <see cref="CspReportController" /> as
///     <c>report-uri</c>.
/// </summary>
public static class CspBuilderReportExtensions
{
    /// <summary>
    ///     Adds the <see cref="CspReportController" /> endpoint as <c>report-uri</c>.
    /// </summary>
    /// <param name="cspBuilder">A <see cref="CspBuilder" /> to add the report directive to.</param>
    /// <returns>The cspBuilder that was passed as this.</returns>
    public static CspDirectiveBuilderBase AddBmReportController(this CspBuilder cspBuilder)
    {
        return cspBuilder.AddBmReportController("");
    }

    /// <summary>
    ///     Adds a <see cref="CspReportController" /> on a different origin as endpoint for <c>report-uri</c>. This can be
    ///     useful if the report controller is located on a different server. This method only needs the origin and
    ///     automatically adds the rest.
    /// </summary>
    /// <param name="cspBuilder">A <see cref="CspBuilder" /> to add the report directive to.</param>
    /// <param name="hostOrigin">The origin of the target reporting endpoint, e.g. <c>https://api.example.com</c>.</param>
    /// <returns>The cspBuilder that was passed as this.</returns>
    public static CspDirectiveBuilderBase AddBmReportController(this CspBuilder cspBuilder, string hostOrigin)
    {
        return cspBuilder.AddReportUri().To($"{hostOrigin}/CspReport");
    }
}