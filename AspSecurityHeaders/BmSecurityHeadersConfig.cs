using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;

namespace Brickmakers.AspSecurityHeaders;

public class BmSecurityHeadersConfig : HeaderPolicyCollection
{
    // ReSharper disable once InconsistentNaming
    internal readonly List<Action<AppendCookieContext>> AddActions = new();
    internal readonly List<Action<DeleteCookieContext>> DeleteActions = new();
    internal Func<HttpContext, bool>? CheckConsentNeeded = null;
    internal CookieBuilder? ConsentCookieBuilder = null;
    internal SameSiteMode MinimumSameSitePolicy = SameSiteMode.Strict;

    internal CookiePolicyOptions CreateCookiePolicy()
    {
        var options = new CookiePolicyOptions
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
            CheckConsentNeeded = CheckConsentNeeded
        };

        if (ConsentCookieBuilder != null)
        {
            options.ConsentCookie = ConsentCookieBuilder;
        }

        return options;
    }
}