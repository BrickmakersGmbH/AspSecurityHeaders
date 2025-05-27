using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;

namespace Brickmakers.AspSecurityHeaders.BmCookiePolicyExtensions;

/// <summary>
///     Extensions to <see cref="BmSecurityHeadersConfig" /> to configure cookie policies.
/// </summary>
public static class BmCookiePolicyExtensions
{
    /// <summary>
    ///     Sets the <see cref="CookiePolicyOptions.MinimumSameSitePolicy" />.
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="BmSecurityHeadersConfig" /> to add cookies to.</param>
    /// <param name="sameSiteMode">The <see cref="SameSiteMode" /> to be used as required minimum.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    /// <exception cref="ArgumentException">
    ///     Is thrown if the headerPolicyCollection is not a <see cref="BmSecurityHeadersConfig" />.
    /// </exception>
    public static BmSecurityHeadersConfig SetMinimumSameSitePolicy(
        this HeaderPolicyCollection headerPolicyCollection,
        SameSiteMode sameSiteMode
    )
    {
        var bmConfig = headerPolicyCollection.AsBmConfig();
        bmConfig.MinimumSameSitePolicy = sameSiteMode;
        return bmConfig;
    }

    /// <summary>
    ///     Adds additional options to be configured per cookie.
    ///     <p>
    ///         This can be used to configure individual cookies in case they need to be configured differently then the
    ///         default settings this library applies.
    ///     </p>
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="BmSecurityHeadersConfig" /> to add cookies to.</param>
    /// <param name="cookieName">The name of the cookie to be modified.</param>
    /// <param name="configure">A callback that modifies the <see cref="CookieOptions" /> for that cookie.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    /// <exception cref="ArgumentException">
    ///     Is thrown if the headerPolicyCollection is not a <see cref="BmSecurityHeadersConfig" />.
    /// </exception>
    /// <seealso cref="CookiePolicyOptions.OnAppendCookie" />
    public static BmSecurityHeadersConfig AddCookieOption(
        this HeaderPolicyCollection headerPolicyCollection,
        string cookieName,
        Action<CookieOptions> configure
    )
    {
        return headerPolicyCollection.AddCookieOption(
            new Regex($"^{Regex.Escape(cookieName)}$"),
            configure
        );
    }

    /// <summary>
    ///     Adds additional options to be configured for cookies matching.
    ///     <p>
    ///         This can be used to configure individual cookies in case they need to be configured differently then the
    ///         default settings this library applies.
    ///     </p>
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="BmSecurityHeadersConfig" /> to add cookies to.</param>
    /// <param name="cookieMatcher">A regular expressions to be matched against added cookies.</param>
    /// <param name="configure">A callback that modifies the <see cref="CookieOptions" /> for that cookie.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    /// <exception cref="ArgumentException">
    ///     Is thrown if the headerPolicyCollection is not a <see cref="BmSecurityHeadersConfig" />.
    /// </exception>
    /// <seealso cref="CookiePolicyOptions.OnAppendCookie" />
    public static BmSecurityHeadersConfig AddCookieOption(
        this HeaderPolicyCollection headerPolicyCollection,
        Regex cookieMatcher,
        Action<CookieOptions> configure
    )
    {
        return headerPolicyCollection.AddCookieFilter(context =>
        {
            if (cookieMatcher.IsMatch(context.CookieName))
            {
                configure(context.CookieOptions);
            }
        });
    }

    /// <summary>
    ///     Adds additional options to be configured for all cookies.
    ///     <p>
    ///         This can be used to configure individual cookies in case they need to be configured differently then the
    ///         default settings this library applies.
    ///     </p>
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="BmSecurityHeadersConfig" /> to add cookies to.</param>
    /// <param name="configure">
    ///     A callback that receives the <see cref="AppendCookieContext" /> for each cookie set. Can be
    ///     used to identify cookies and modify their configuration.
    /// </param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    /// <exception cref="ArgumentException">
    ///     Is thrown if the headerPolicyCollection is not a <see cref="BmSecurityHeadersConfig" />.
    /// </exception>
    /// <seealso cref="CookiePolicyOptions.OnAppendCookie" />
    public static BmSecurityHeadersConfig AddCookieFilter(
        this HeaderPolicyCollection headerPolicyCollection,
        Action<AppendCookieContext> configure
    )
    {
        var bmConfig = headerPolicyCollection.AsBmConfig();
        bmConfig.AddActions.Add(configure);
        return bmConfig;
    }

    /// <summary>
    ///     Adds a callback that is invoked whenever cookies are removed
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="BmSecurityHeadersConfig" /> to add cookies to.</param>
    /// <param name="configure">
    ///     A callback that receives the <see cref="DeleteCookieContext" /> for each cookie removed. Can be
    ///     used to identify cookies and modify their configuration.
    /// </param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    /// <exception cref="ArgumentException">
    ///     Is thrown if the headerPolicyCollection is not a <see cref="BmSecurityHeadersConfig" />.
    /// </exception>
    /// <seealso cref="CookiePolicyOptions.OnDeleteCookie" />
    public static BmSecurityHeadersConfig AddDeleteCookieFilter(
        this HeaderPolicyCollection headerPolicyCollection,
        Action<DeleteCookieContext> configure
    )
    {
        var bmConfig = headerPolicyCollection.AsBmConfig();
        bmConfig.DeleteActions.Add(configure);
        return bmConfig;
    }

    /// <summary>
    ///     Sets a <see cref="CookieBuilder" /> that is used to generate a consent cookie.
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="BmSecurityHeadersConfig" /> to add cookies to.</param>
    /// <param name="builder">The cookie builder for the consent cookie.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    /// <exception cref="ArgumentException">
    ///     Is thrown if the headerPolicyCollection is not a <see cref="BmSecurityHeadersConfig" />.
    /// </exception>
    /// <seealso cref="CookiePolicyOptions.ConsentCookie" />
    public static BmSecurityHeadersConfig SetConsentCookie(
        this HeaderPolicyCollection headerPolicyCollection,
        CookieBuilder builder
    )
    {
        var bmConfig = headerPolicyCollection.AsBmConfig();
        bmConfig.ConsentCookieBuilder = builder;
        return bmConfig;
    }

    /// <summary>
    ///     Sets a callback that is used to determine if content is needed for a <see cref="HttpContext" />
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="BmSecurityHeadersConfig" /> to add cookies to.</param>
    /// <param name="checkConsentNeeded">A callback that is invoked to determine if consent is needed.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    /// <exception cref="ArgumentException">
    ///     Is thrown if the headerPolicyCollection is not a <see cref="BmSecurityHeadersConfig" />.
    /// </exception>
    /// <seealso cref="CookiePolicyOptions.CheckConsentNeeded" />
    public static BmSecurityHeadersConfig CheckConsentNeeded(
        this HeaderPolicyCollection headerPolicyCollection,
        Func<HttpContext, bool> checkConsentNeeded
    )
    {
        var bmConfig = headerPolicyCollection.AsBmConfig();
        bmConfig.CheckConsentNeeded = checkConsentNeeded;
        return bmConfig;
    }

    private static BmSecurityHeadersConfig AsBmConfig(
        this HeaderPolicyCollection headerPolicyCollection
    )
    {
        if (headerPolicyCollection is BmSecurityHeadersConfig bmConfig)
        {
            return bmConfig;
        }

        throw new ArgumentException("this is not a BmSecurityHeadersConfig");
    }
}
