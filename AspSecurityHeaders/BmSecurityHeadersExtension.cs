using System;
using de.brickmakers.SecurityEngineering.AspSecurityHeaders.HeaderPolicyCollectionExtensions;
using Microsoft.AspNetCore.Builder;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders
{
    public static class BmSecurityHeadersExtension
    {
        public static void UseBmSecurityHeaders(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseSecurityHeaders(policy => policy.AddDefaultBmSecurityHeaders());
        }

        public static void UseBmSecurityHeaders(this IApplicationBuilder applicationBuilder,
            Action<HeaderPolicyCollection> configure)
        {
            applicationBuilder.UseSecurityHeaders(policy => configure(policy.AddDefaultBmSecurityHeaders()));
        }
    }
}