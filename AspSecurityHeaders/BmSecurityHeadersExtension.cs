﻿using System;
using de.brickmakers.SecurityEngineering.AspSecurityHeaders.HeaderPolicyCollectionExtensions;
using Microsoft.AspNetCore.Builder;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders
{
    public static class BmSecurityHeadersExtension
    {
        public static void UseBmSecurityHeaders(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseBmSecurityHeaders(policy => {});
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
    }
}