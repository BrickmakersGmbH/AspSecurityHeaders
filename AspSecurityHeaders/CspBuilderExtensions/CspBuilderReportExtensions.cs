using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.ContentSecurityPolicy;

namespace Brickmakers.AspSecurityHeaders.CspBuilderExtensions;

public static class CspBuilderReportExtensions
{
    public static CspDirectiveBuilderBase AddBmReportController(this CspBuilder cspBuilder)
    {
        return cspBuilder.AddBmReportController("");
    }

    public static CspDirectiveBuilderBase AddBmReportController(this CspBuilder cspBuilder, string hostOrigin)
    {
        return cspBuilder.AddReportUri().To($"{hostOrigin}/CspReport");
    }
}