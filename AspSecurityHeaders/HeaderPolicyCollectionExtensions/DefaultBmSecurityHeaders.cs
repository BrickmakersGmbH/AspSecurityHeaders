using Microsoft.AspNetCore.Builder;

namespace Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;

/// <summary>
///     Extensions to <see cref="HeaderPolicyCollection" /> to add all default security headers with save values. Should be
///     used as basis for all security header configurations, as headers can be individually modified if needed.
/// </summary>
public static class DefaultBmSecurityHeaders
{
    /// <summary>
    ///     Adds all default security headers for HTTP-Websites. These headers are:
    ///     <code>
    ///         Strict-Transport-Security: max-age=31536000; includeSubDomains
    ///         Cross-Origin-Embedder-Policy: require-corp
    ///         Cross-Origin-Opener-Policy: same-origin
    ///         Cross-Origin-Resource-Policy: same-origin
    ///         X-Frame-Options: DENY
    ///         X-Content-Type-Options: nosniff
    ///         X-Permitted-Cross-Domain-Policies: none
    ///         X-XSS-Protection: 0
    ///         Referrer-Policy: no-referrer
    ///         Cache-Control: no-store
    ///         Content-Security-Policy: ...
    ///         Permissions-Policy: ...
    ///         Feature-Policy: ...
    ///         &lt;removes&gt; Server
    ///         &lt;removes&gt; X-Powered-By
    ///     </code>
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the headers to.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    public static HeaderPolicyCollection AddDefaultBmSecurityHeaders(
        this HeaderPolicyCollection headerPolicyCollection
    )
    {
        return headerPolicyCollection
            .AddDefaultSecurityHeaders()
            .AddStrictTransportSecurityMaxAgeIncludeSubDomains()
            .AddReferrerPolicyNoReferrer()
            .AddXPermittedCrossDomainPoliciesNone()
            .AddCrossOriginEmbedderPolicy(builder => builder.RequireCorp())
            .AddCrossOriginOpenerPolicy(builder => builder.SameOrigin())
            .AddCrossOriginResourcePolicy(builder => builder.SameOrigin())
            .AddCacheControlNoStore()
            .AddBmContentSecurityPolicy(builder => { })
            .AddBmPermissionPolicy(builder => { })
            .RemoveServerHeader()
            .RemoveCustomHeader("X-Powered-By");
    }

    /// <summary>
    ///     Adds all default security headers for REST-APIs. These headers are:
    ///     <code>
    ///         Strict-Transport-Security: max-age=31536000; includeSubDomains
    ///         X-Content-Type-Options: nosniff
    ///         X-Permitted-Cross-Domain-Policies: none
    ///         Cache-Control: no-store
    ///         &lt;removes&gt; Server
    ///     </code>
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the headers to.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    public static HeaderPolicyCollection AddDefaultBmApiSecurityHeaders(
        this HeaderPolicyCollection headerPolicyCollection
    )
    {
        return headerPolicyCollection
            .AddDefaultApiSecurityHeaders()
            .AddStrictTransportSecurityMaxAgeIncludeSubDomains()
            .AddXPermittedCrossDomainPoliciesNone()
            .AddCacheControlNoStore()
            .RemoveServerHeader();
    }
}
