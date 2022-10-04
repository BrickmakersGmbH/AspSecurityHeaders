using Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;
using Microsoft.AspNetCore.Builder;

namespace Brickmakers.AspSecurityHeaders;

/// <summary>
///     Extensions to <see cref="IApplicationBuilder" /> to add the security headers middleware and configure it.
/// </summary>
public static class BmSecurityHeadersExtension
{
    /// <summary>
    ///     Adds the security headers middleware with the default configuration.
    /// </summary>
    /// <param name="applicationBuilder">A <see cref="IApplicationBuilder" /> to add the middleware to.</param>
    /// <returns>The applicationBuilder that was passed as this.</returns>
    public static IApplicationBuilder UseBmSecurityHeaders(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseBmSecurityHeaders(policy => { });
    }

    /// <summary>
    ///     Adds the security headers middleware with the default configuration and allows you to further configure them.
    /// </summary>
    /// <param name="applicationBuilder">A <see cref="IApplicationBuilder" /> to add the middleware to.</param>
    /// <param name="configure">
    ///     A callback that gets passed a <see cref="BmSecurityHeadersConfig" /> that can be used to
    ///     configure the security headers middleware.
    /// </param>
    /// <returns>The applicationBuilder that was passed as this.</returns>
    public static IApplicationBuilder UseBmSecurityHeaders(this IApplicationBuilder applicationBuilder,
        Action<BmSecurityHeadersConfig> configure)
    {
        var policy = new BmSecurityHeadersConfig();
        policy.AddDefaultBmSecurityHeaders();
        configure(policy);
        applicationBuilder.UseSecurityHeaders(policy);
        applicationBuilder.UseCookiePolicy(policy.CreateCookiePolicy());
        return applicationBuilder;
    }

    /// <summary>
    ///     Adds the security headers middleware with the default configuration for REST-APIs.
    /// </summary>
    /// <param name="applicationBuilder">A <see cref="IApplicationBuilder" /> to add the middleware to.</param>
    /// <returns>The applicationBuilder that was passed as this.</returns>
    public static IApplicationBuilder UseBmApiSecurityHeaders(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseBmApiSecurityHeaders(policy => { });
    }

    /// <summary>
    ///     Adds the security headers middleware with the default configuration for REST-APIs and allows you to further
    ///     configure them.
    /// </summary>
    /// <param name="applicationBuilder">A <see cref="IApplicationBuilder" /> to add the middleware to.</param>
    /// <param name="configure">
    ///     A callback that gets passed a <see cref="BmSecurityHeadersConfig" /> that can be used to
    ///     configure the security headers middleware.
    /// </param>
    /// <returns>The applicationBuilder that was passed as this.</returns>
    public static IApplicationBuilder UseBmApiSecurityHeaders(this IApplicationBuilder applicationBuilder,
        Action<BmSecurityHeadersConfig> configure)
    {
        var policy = new BmSecurityHeadersConfig();
        policy.AddDefaultBmApiSecurityHeaders();
        configure(policy);
        applicationBuilder.UseSecurityHeaders(policy);
        applicationBuilder.UseCookiePolicy(policy.CreateCookiePolicy());
        return applicationBuilder;
    }
}