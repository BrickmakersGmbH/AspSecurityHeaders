using System.Text.RegularExpressions;
using Brickmakers.AspSecurityHeaders.BmCookiePolicyExtensions;
using Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Brickmakers.AspSecurityHeaders.OrchardModule;

/// <summary>
///     Extensions to <see cref="IApplicationBuilder" /> to add the orchard security headers middleware and configure it.
/// </summary>
public static class OrchardSecurityHeaders
{
    /// <summary>
    ///     The default version of the fontawesome package as currently used by Orchard Core 1.5.0.
    /// </summary>
    public const string DefaultFontawesomeVersion = "6.2.0";


    /// <summary>
    ///     Configure the security headers middleware with the default configuration plus your customizations.
    /// </summary>
    /// <param name="app">A <see cref="IApplicationBuilder" /> to add the middleware to.</param>
    /// <param name="configure">
    ///     A callback that gets passed a <see cref="BmSecurityHeadersConfig" /> that can be used to
    ///     configure the security headers middleware.
    /// </param>
    /// <returns>The applicationBuilder that was passed as this.</returns>
    /// <remarks>
    ///     If the orchard module itself is activated, the headers get configured automatically. Unless you do need to
    ///     customize the headers, this call is not needed.
    /// </remarks>
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


    /// <summary>
    ///     Adds special cookie policy rules to enable the login to the Azure AD.<br />
    ///     This will ensure that special Azure login cookies are allowed to have SameSite=None, as the default cookie policy
    ///     for orchard projects (SameSize=Lax) would otherwise prevent them from being returned from the azure login.
    /// </summary>
    /// <param name="headerPolicyCollection">to add the rules to.</param>
    /// <returns>The headerPolicyCollection that was passed to this.</returns>
    /// <remarks>Unless you actually use the azure login, you do not need to enable this.</remarks>
    public static HeaderPolicyCollection AddAzureLoginCookieWhitelist(
        this HeaderPolicyCollection headerPolicyCollection)
    {
        return headerPolicyCollection
            .AddCookieOption(new Regex(@"^\.AspNetCore\.OpenIdConnect\.Nonce\..+$"),
                options => options.SameSite = SameSiteMode.None)
            .AddCookieOption(new Regex(@"^\.AspNetCore\.Correlation\..+$"),
                options => options.SameSite = SameSiteMode.None);
    }


    /// <summary>
    ///     Adds a CSP-Header to the security headers with the default secure basis of CSP-directives plus all the
    ///     configuration that orchard itself needs to work. Please note that this includes unsafe-inline and unsafe-eval for
    ///     scripts and styles. Thus, a CSP has only very limited effectiveness for Orchard Projects.<br />
    ///     The standard directives are:
    ///     <code>
    ///         default-src 'none';
    ///         base-uri 'none';
    ///         form-action 'self';
    ///         frame-ancestors 'none';
    ///         script-src 'self' 'unsafe-inline' 'unsafe-eval' 'report-sample';
    ///         style-src 'self' https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@6.2.0/css/ 'unsafe-inline' 'report-sample';
    ///         img-src 'self' data:
    ///         font-src 'self' https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@6.2.0/webfonts/
    ///         connect-src 'self'
    ///         upgrade-insecure-requests;  // if allowInsecureRequests is false (default)
    ///         block-all-mixed-content;  // if allowMixedContent is false (default)
    ///     </code>
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the CSP to.</param>
    /// <param name="cspBuilder">
    ///     A configure callback that provides a <see cref="CspBuilder" /> to add directives to. The build is
    ///     already preconfigured with a secure basis of CSP directives.
    /// </param>
    /// <param name="fontawesomeVersion">
    ///     If specified, the given fontawesome version is used instead of the
    ///     <see cref="DefaultFontawesomeVersion" /> to build the CSP.
    /// </param>
    /// <param name="allowInsecureRequests">If set to true, the <c>upgrade-insecure-requests</c> directive is not set.</param>
    /// <param name="allowMixedContent">If set to true, the <c>block-all-mixed-content</c> directive is not set.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    /// <remarks>
    ///     You can easily overwrite any of the default directives by calling <c>builder.AddXXX()</c> again. For example,
    ///     to use a custom script-src, use:
    ///     <code>
    ///         builder => builder.AddScriptSrc().Self().ReportSample();
    ///     </code>
    /// </remarks>
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