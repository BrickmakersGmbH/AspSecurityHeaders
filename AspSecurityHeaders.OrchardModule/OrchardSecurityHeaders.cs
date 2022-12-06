using System.Text.RegularExpressions;
using Brickmakers.AspSecurityHeaders.BmCookiePolicyExtensions;
using Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Brickmakers.AspSecurityHeaders.OrchardModule;

public static class OrchardSecurityHeaders
{
    public const string DefaultFontawesomeVersion = "6.2.0";

    public static IApplicationBuilder UseOrchardBmSecurityHeaders(this IApplicationBuilder app)
    {
        return app.UseOrchardBmSecurityHeaders(config => { });
    }


    public static IApplicationBuilder UseOrchardBmSecurityHeaders(this IApplicationBuilder app,
        Action<BmSecurityHeadersConfig> configure)
    {
        return app.UseBmSecurityHeaders(config =>
        {
            config
                .SetMinimumSameSitePolicy(SameSiteMode.Lax)
                .AddOrchardBmContentSecurityPolicy(builder => { });
            configure(config);
        });
    }


    public static HeaderPolicyCollection AddAzureLoginCookieWhitelist(
        this HeaderPolicyCollection headerPolicyCollection)
    {
        return headerPolicyCollection
            .AddCookieOption(new Regex(@"^\.AspNetCore\.OpenIdConnect\.Nonce\..+$"),
                options => options.SameSite = SameSiteMode.None)
            .AddCookieOption(new Regex(@"^\.AspNetCore\.Correlation\..+$"),
                options => options.SameSite = SameSiteMode.None);
    }


    public static HeaderPolicyCollection AddOrchardBmContentSecurityPolicy(
        this HeaderPolicyCollection headerPolicyCollection,
        Action<CspBuilder> cspBuilder,
        string? fontawesomeVersion = null,
        bool allowInsecureRequests = false,
        bool allowMixedContent = false)
    {
        var fontawesomeBaseUrl =
            $"https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@{fontawesomeVersion ?? DefaultFontawesomeVersion}";
        return headerPolicyCollection.AddBmContentSecurityPolicy(builder =>
            {
                builder.AddScriptSrc()
                    .Self()
                    .UnsafeInline()
                    .UnsafeEval()
                    .ReportSample();
                builder.AddImgSrc()
                    .Self()
                    .Data();
                builder.AddStyleSrc()
                    .Self()
                    .From($"{fontawesomeBaseUrl}/css/")
                    .UnsafeInline()
                    .ReportSample();
                builder.AddFontSrc()
                    .Self()
                    .From($"{fontawesomeBaseUrl}/webfonts/");
                builder.AddFormAction().Self();
                builder.AddConnectSrc().Self();
                builder.AddReportUri().To("/CspReport");

                cspBuilder(builder);
            },
            allowInsecureRequests,
            allowMixedContent);
    }
}