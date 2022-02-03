using Microsoft.AspNetCore.Builder;

namespace Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;

/// <summary>
///     Extensions to <see cref="HeaderPolicyCollection" /> to configure the content security policy (CSP)
/// </summary>
public static class BmContentSecurityPolicy
{
    /// <summary>
    ///     Adds a CSP-Header to the security headers with the default secure basis of CSP-directives, combined with however
    ///     the CSP is configured afterwards. The standard directives are:
    ///     <code>
    ///         default-src 'none';
    ///         base-uri 'none';
    ///         form-action 'none';
    ///         frame-ancestors 'none';
    ///         script-src 'none' 'report-sample';
    ///         style-src 'none' 'report-sample';
    ///         upgrade-insecure-requests;  // if allowInsecureRequests is false (default)
    ///         block-all-mixed-content;  // if allowMixedContent is false (default)
    ///     </code>
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the CSP to.</param>
    /// <param name="cspBuilder">
    ///     A configure callback that provides a <see cref="CspBuilder" /> to add directives to. The build is
    ///     already preconfigured with a secure basis of CSP directives.
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
    public static HeaderPolicyCollection AddBmContentSecurityPolicy(
        this HeaderPolicyCollection headerPolicyCollection, Action<CspBuilder> cspBuilder,
        bool allowInsecureRequests = false, bool allowMixedContent = false)
    {
        return headerPolicyCollection.AddContentSecurityPolicy(builder =>
        {
            builder.AddDefaultSrc().None();
            builder.AddBaseUri().None();
            builder.AddFormAction().None();
            builder.AddFrameAncestors().None();
            builder.AddScriptSrc().None().ReportSample();
            builder.AddStyleSrc().None().ReportSample();
            if (!allowInsecureRequests)
            {
                builder.AddUpgradeInsecureRequests();
            }

            if (!allowMixedContent)
            {
                builder.AddBlockAllMixedContent();
            }

            cspBuilder(builder);
        });
    }
}