using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders
{
    public class BmSecurityHeadersConfig : HeaderPolicyCollection
    {
        internal SameSiteMode MinimumSameSitePolicy = SameSiteMode.Strict;
        internal readonly List<Action<AppendCookieContext>> AddActions = new();
        internal readonly List<Action<DeleteCookieContext>> DeleteActions = new();
        internal CookieBuilder? ConsentCookieBuilder = null;
        internal Func<HttpContext, bool>? CheckConsentNeeded = null;

        internal CookiePolicyOptions CreateCookiePolicy()
        {
            return new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always,
                HttpOnly = HttpOnlyPolicy.Always,
                MinimumSameSitePolicy = MinimumSameSitePolicy,
                OnAppendCookie = AddActions.Count > 0
                    ? context => AddActions.ForEach(configure => configure(context))
                    : null,
                OnDeleteCookie = DeleteActions.Count > 0
                    ? context => DeleteActions.ForEach(configure => configure(context))
                    : null,
                ConsentCookie = ConsentCookieBuilder,
                CheckConsentNeeded = CheckConsentNeeded,
            };
        }
    }
}