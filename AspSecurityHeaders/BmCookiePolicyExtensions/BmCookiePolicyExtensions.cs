using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;

namespace Brickmakers.AspSecurityHeaders.BmCookiePolicyExtensions
{
    public static class BmCookiePolicyExtensions
    {
        public static BmSecurityHeadersConfig SetMinimumSameSitePolicy(
            this HeaderPolicyCollection headerPolicyCollection, SameSiteMode sameSiteMode)
        {
            var bmConfig = headerPolicyCollection.AsBmConfig();
            bmConfig.MinimumSameSitePolicy = sameSiteMode;
            return bmConfig;
        }
        
        public static BmSecurityHeadersConfig AddCookieOption(this HeaderPolicyCollection headerPolicyCollection,
            string cookieName, Action<CookieOptions> configure)
        {
            return headerPolicyCollection.AddCookieOption(new Regex($"^{Regex.Escape(cookieName)}$"), configure);
        }

        public static BmSecurityHeadersConfig AddCookieOption(this HeaderPolicyCollection headerPolicyCollection,
            Regex cookieMatcher, Action<CookieOptions> configure)
        {
            return headerPolicyCollection.AddCookieFilter(context =>
            {
                if (cookieMatcher.IsMatch(context.CookieName))
                {
                    configure(context.CookieOptions);
                }
            });
        }

        public static BmSecurityHeadersConfig AddCookieFilter(this HeaderPolicyCollection headerPolicyCollection,
            Action<AppendCookieContext> configure)
        {
            var bmConfig = headerPolicyCollection.AsBmConfig();
            bmConfig.AddActions.Add(configure);
            return bmConfig;
        }

        public static BmSecurityHeadersConfig AddDeleteCookieFilter(this HeaderPolicyCollection headerPolicyCollection,
            Action<DeleteCookieContext> configure)
        {
            var bmConfig = headerPolicyCollection.AsBmConfig();
            bmConfig.DeleteActions.Add(configure);
            return bmConfig;
        }

        public static BmSecurityHeadersConfig SetConsentCookie(this HeaderPolicyCollection headerPolicyCollection,
            CookieBuilder builder)
        {
            var bmConfig = headerPolicyCollection.AsBmConfig();
            bmConfig.ConsentCookieBuilder = builder;
            return bmConfig;
        }

        public static BmSecurityHeadersConfig CheckConsentNeeded(this HeaderPolicyCollection headerPolicyCollection,
            Func<HttpContext, bool> checkConsentNeeded)
        {
            var bmConfig = headerPolicyCollection.AsBmConfig();
            bmConfig.CheckConsentNeeded = checkConsentNeeded;
            return bmConfig;
        }

        private static BmSecurityHeadersConfig AsBmConfig(this HeaderPolicyCollection headerPolicyCollection)
        {
            if (headerPolicyCollection is BmSecurityHeadersConfig bmConfig)
            {
                return bmConfig;
            }
            
            throw new ArgumentException("this is not a BmSecurityHeadersConfig");
        }
    }
}