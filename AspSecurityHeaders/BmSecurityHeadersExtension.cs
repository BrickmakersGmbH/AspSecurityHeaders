using Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;
using Microsoft.AspNetCore.Builder;

namespace Brickmakers.AspSecurityHeaders;

public static class BmSecurityHeadersExtension
{
    public static void UseBmSecurityHeaders(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseBmSecurityHeaders(policy => { });
    }

    public static void UseBmSecurityHeaders(this IApplicationBuilder applicationBuilder,
        Action<BmSecurityHeadersConfig> configure)
    {
        var policy = new BmSecurityHeadersConfig();
        policy.AddDefaultBmSecurityHeaders();
        configure(policy);
        applicationBuilder.UseSecurityHeaders(policy);
        applicationBuilder.UseCookiePolicy(policy.CreateCookiePolicy());
    }

    public static void UseBmApiSecurityHeaders(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseBmApiSecurityHeaders(policy => { });
    }

    public static void UseBmApiSecurityHeaders(this IApplicationBuilder applicationBuilder,
        Action<BmSecurityHeadersConfig> configure)
    {
        var policy = new BmSecurityHeadersConfig();
        policy.AddDefaultBmApiSecurityHeaders();
        configure(policy);
        applicationBuilder.UseSecurityHeaders(policy);
        applicationBuilder.UseCookiePolicy(policy.CreateCookiePolicy());
    }
}