using System;
using System.Linq;
using de.brickmakers.SecurityEngineering.AspSecurityHeaders.HeaderPolicyCollectionExtensions;
using Microsoft.AspNetCore.Builder;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders
{
    public static class BmSecurityHeadersExtension
    {
        public static void UseBmSecurityHeaders(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseBmSecurityHeaders(collection => {});
        }

        public static void UseBmSecurityHeaders(this IApplicationBuilder applicationBuilder,
            Action<HeaderPolicyCollection> configure)
        {
            var policy = new HeaderPolicyCollection()
                .AddDefaultSecurityHeaders()
                .AddStrictTransportSecurityMaxAgeIncludeSubDomainsAndPreload()
                .AddXssProtectionDisabled()
                .AddReferrerPolicyNoReferrer()
                .AddXPermittedCrossDomainPoliciesNone()
                .AddCacheControlNoStore()
                .RemoveServerHeader();
            configure(policy);
            applicationBuilder.UseSecurityHeaders(policy);
        }
    }
}