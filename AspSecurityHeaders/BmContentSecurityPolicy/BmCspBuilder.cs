using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.ContentSecurityPolicy;

namespace Brickmakers.AspSecurityHeaders.BmContentSecurityPolicy;

/// <summary>
///     Used to build a CSP header from multiple directives.
///     This class wraps <see cref="CspBuilder" /> to allow incremental configuration of the directives. This means if you
///     do the following:
///     <code>
///         builder.AddScriptSrc().Self();
///         builder.AddScriptSrc().From("https://example.com");
///     </code>
///     This builder will create a CSP that contains: <c>script-src: 'self' https://example.com</c> by merging all
///     configuration values incrementally. The raw <see cref="CspBuilder" /> would only generate
///     <c>script-src: https://example.com</c>, as each add clears the previous one. If you need to actually clear old
///     values before setting new ones, you can use the optional <c>clear</c> parameter that is provided on all methods.
/// </summary>
/// <remarks>
///     Be aware that mixing default and custom directives with the same target can lead to unexpected results! For
///     example, the following code would only produce <c>script-src: 'self'</c>:
///     <code>
///         builder.AddScriptSrc().From("https://sub1.example.com");
///         builder.AddCustomDirective("script-src", "'self'");
///         builder.AddScriptSrc().From("https://sub2.example.com");
///     </code>
///     For this reason, you should NEVER use <see cref="AddCustomDirective(string,bool)" /> or
///     <see cref="AddCustomDirectiveBuilder" /> for one of the default directives!
/// </remarks>
public class BmCspBuilder
{
    private readonly Dictionary<string, CspDirectiveBuilderBase> _directiveCache = new();

    internal BmCspBuilder(CspBuilder rawCspBuilder)
    {
        RawCspBuilder = rawCspBuilder;
    }

    /// <summary>
    ///     The <see cref="CspBuilder" /> that is wrapped by this instance. Can be accessed if direct interaction with
    ///     the raw builder is needed.
    /// </summary>
    public CspBuilder RawCspBuilder { get; }

    /// <summary>
    ///     The default-src directive serves as a fallback for the other CSP fetch directives.
    ///     Valid sources include 'self', 'unsafe-inline', 'unsafe-eval', 'none', scheme such as http:,
    ///     or internet hosts by name or IP address, as well as an optional URL scheme and/or port number.
    ///     The site's address may include an optional leading wildcard (the asterisk character, '*'), and
    ///     you may use a wildcard (again, '*') as the port number, indicating that all legal ports are valid for the source.
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="DefaultSourceDirectiveBuilder" /></returns>
    public DefaultSourceDirectiveBuilder AddDefaultSrc(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddDefaultSrc), RawCspBuilder.AddDefaultSrc, clear);
    }

    /// <summary>
    ///     The connect-src directive restricts the URLs which can be loaded using script interfaces
    ///     The APIs that are restricted are:  &lt;a&gt; ping, Fetch, XMLHttpRequest, WebSocket, and EventSource.
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="ConnectSourceDirectiveBuilder" /></returns>
    public ConnectSourceDirectiveBuilder AddConnectSrc(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddConnectSrc), RawCspBuilder.AddConnectSrc, clear);
    }

    /// <summary>
    ///     The font-src directive specifies valid sources for fonts loaded using @font-face.
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="FontSourceDirectiveBuilder" /></returns>
    public FontSourceDirectiveBuilder AddFontSrc(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddFontSrc), RawCspBuilder.AddFontSrc, clear);
    }

    /// <summary>
    ///     The object-src directive specifies valid sources for the &lt;object&gt;, &lt;embed&gt;, and &lt;applet&gt; elements
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="ObjectSourceDirectiveBuilder" /></returns>
    public ObjectSourceDirectiveBuilder AddObjectSrc(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddObjectSrc), RawCspBuilder.AddObjectSrc, clear);
    }

    /// <summary>
    ///     The form-action directive restricts the URLs which can be used as the target of a form submissions from a given
    ///     context
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="FormActionDirectiveBuilder" /></returns>
    public FormActionDirectiveBuilder AddFormAction(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddFormAction), RawCspBuilder.AddFormAction, clear);
    }

    /// <summary>
    ///     The img-src directive specifies valid sources of images and favicons
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="ImageSourceDirectiveBuilder" /></returns>
    public ImageSourceDirectiveBuilder AddImgSrc(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddImgSrc), RawCspBuilder.AddImgSrc, clear);
    }

    /// <summary>
    ///     The script-src directive specifies valid sources for sources for JavaScript.
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="ScriptSourceDirectiveBuilder" /></returns>
    public ScriptSourceDirectiveBuilder AddScriptSrc(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddScriptSrc), RawCspBuilder.AddScriptSrc, clear);
    }

    /// <summary>
    ///     The style-src directive specifies valid sources for sources for stylesheets.
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="StyleSourceDirectiveBuilder" /></returns>
    public StyleSourceDirectiveBuilder AddStyleSrc(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddStyleSrc), RawCspBuilder.AddStyleSrc, clear);
    }

    /// <summary>
    ///     The media-src directive specifies valid sources for loading media using the &lt;audio&gt; and &lt;video&gt;
    ///     elements
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="MediaSourceDirectiveBuilder" /></returns>
    public MediaSourceDirectiveBuilder AddMediaSrc(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddMediaSrc), RawCspBuilder.AddMediaSrc, clear);
    }

    /// <summary>
    ///     The manifest-src directive specifies which manifest can be applied to the resource.
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="ManifestSourceDirectiveBuilder" /></returns>
    public ManifestSourceDirectiveBuilder AddManifestSrc(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddManifestSrc), RawCspBuilder.AddManifestSrc, clear);
    }

    /// <summary>
    ///     The frame-ancestors directive specifies valid parents that may embed a page using
    ///     &lt;frame&gt;, &lt;iframe&gt;, &lt;object&gt;, &lt;embed&gt;, or &lt;applet&gt;.
    ///     Setting this directive to 'none' is similar to X-Frame-Options: DENY (which is also supported in older browsers).
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="FrameAncestorsDirectiveBuilder" /></returns>
    public FrameAncestorsDirectiveBuilder AddFrameAncestors(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddFrameAncestors), RawCspBuilder.AddFrameAncestors, clear);
    }

    /// <summary>
    ///     The frame-src directive specifies valid sources for nested browsing contexts loading
    ///     using elements such as  &lt;frame&gt; and  &lt;iframe&gt;
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="FrameSourceDirectiveBuilder" /></returns>
    public FrameSourceDirectiveBuilder AddFrameSrc(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddFrameSrc), RawCspBuilder.AddFrameSrc, clear);
    }

    /// <summary>
    ///     The frame-src directive specifies valid sources for nested browsing contexts loading
    ///     using elements such as  &lt;frame&gt; and  &lt;iframe&gt;
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="FrameSourceDirectiveBuilder" /></returns>
    [Obsolete("Use AddFrameSrc method instead. This method will be removed in a future version of the library.")]
    public FrameSourceDirectiveBuilder AddFrameSource(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddFrameSrc), RawCspBuilder.AddFrameSrc, clear);
    }

    /// <summary>
    ///     The worker-src directive specifies valid sources for Worker, SharedWorker, or ServiceWorker scripts.
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="WorkerSourceDirectiveBuilder" /></returns>
    public WorkerSourceDirectiveBuilder AddWorkerSrc(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddWorkerSrc), RawCspBuilder.AddWorkerSrc, clear);
    }

    /// <summary>
    ///     The base-uri directive restricts the URLs which can be used in a document's
    ///     &lt;base&gt; element. If this value is absent, then any URI is allowed. If this
    ///     directive is absent, the user agent will use the value in the &lt;base&gt; element.
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="BaseUriDirectiveBuilder" /></returns>
    public BaseUriDirectiveBuilder AddBaseUri(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddBaseUri), RawCspBuilder.AddBaseUri, clear);
    }

    /// <summary>
    ///     The sandbox directive enables a sandbox for the requested resource similar
    ///     to the &lt;script&gt; sandbox attribute. It applies restrictions to a
    ///     page's actions including preventing popups, preventing the execution
    ///     of plugins and scripts, and enforcing a same-origin policy.
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="SandboxDirectiveBuilder" /></returns>
    public SandboxDirectiveBuilder AddSandbox(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddSandbox), RawCspBuilder.AddSandbox, clear);
    }

    /// <summary>
    ///     The upgrade-insecure-requests directive instructs user agents to treat all of a
    ///     site's insecure URLs (those served over HTTP) as though they have been
    ///     replaced with secure URLs (those served over HTTPS). This directive is
    ///     intended for web sites with large numbers of insecure legacy URLs that need to be rewritten.
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="UpgradeInsecureRequestsDirectiveBuilder" /></returns>
    public UpgradeInsecureRequestsDirectiveBuilder AddUpgradeInsecureRequests(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddUpgradeInsecureRequests), RawCspBuilder.AddUpgradeInsecureRequests, clear);
    }

    /// <summary>
    ///     The block-all-mixed-content directive prevents loading any assets using
    ///     HTTP when the page is loaded using HTTPS.
    ///     All mixed content resource requests are blocked, including both active
    ///     and passive mixed content. This also applies to &lt;iframe&gt; documents,
    ///     ensuring the entire page is mixed content free.
    ///     The upgrade-insecure-requests directive is evaluated before block-all-mixed-content
    ///     and If the former is set, the latter is effectively a no-op.
    ///     It is recommended to set one directive or the other – not both.
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="BlockAllMixedContentDirectiveBuilder" /></returns>
    public BlockAllMixedContentDirectiveBuilder AddBlockAllMixedContent(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddBlockAllMixedContent), RawCspBuilder.AddBlockAllMixedContent, clear);
    }

    /// <summary>
    ///     The report-uri directive instructs the user agent to report attempts to
    ///     violate the Content Security Policy. These violation reports consist of
    ///     JSON documents sent via an HTTP POST request to the specified URI.
    /// </summary>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="ReportUriDirectiveBuilder" /></returns>
    /// ///
    /// <remarks>
    ///     NOTE: this directive has been deprecated in favour of <c>Report-To</c>.
    ///     Use <see cref="AddReportTo" /> instead.
    /// </remarks>
    public ReportUriDirectiveBuilder AddReportUri(bool clear = false)
    {
        return AddOrGetDirective(nameof(AddReportUri), RawCspBuilder.AddReportUri, clear);
    }

    /// <summary>
    ///     The report-to directive instructs the user agent to send requests to
    ///     an endpoint defined in a <c>Report-To</c> HTTP header. The directive
    ///     has no effect in and of itself, but only gains meaning in
    ///     combination with other reporting directives.
    /// </summary>
    /// <param name="groupName">
    ///     The name of the group in the <code>Report-To</code> JSON field
    ///     to send reports to
    /// </param>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="ReportToDirectiveBuilder" /></returns>
    public ReportToDirectiveBuilder AddReportTo(string groupName, bool clear = false)
    {
        return AddOrGetDirective(nameof(AddReportTo), () => RawCspBuilder.AddReportTo(groupName), clear);
    }

    /// <summary>
    ///     Create a custom CSP directive for an un-implemented directive
    /// </summary>
    /// <param name="directive">The directive name, e.g. default-src</param>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="CustomDirective" /></returns>
    public CustomDirective AddCustomDirective(string directive, bool clear = false)
    {
        return AddOrGetDirective($"{nameof(AddCustomDirective)}({directive})",
            () => RawCspBuilder.AddCustomDirective(directive), clear);
    }

    /// <summary>
    ///     Create a custom CSP directive for an un-implemented directive
    /// </summary>
    /// <param name="directive">The directive name, e.g. default-src</param>
    /// <param name="value">The directive value</param>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <returns>A configured <see cref="CustomDirective" /></returns>
    public CustomDirective AddCustomDirective(string directive, string value, bool clear = false)
    {
        return AddOrGetDirective($"{nameof(AddCustomDirective)}({directive})",
            () => RawCspBuilder.AddCustomDirective(directive, value), clear);
    }

    /// <summary>
    ///     Create a custom CSP directive for an un-implemented directive that uses standard or per-request values such as
    ///     nonce
    ///     To create a custom CSP directive with a fixed value, use <see cref="AddCustomDirective(string,bool)" />
    /// </summary>
    /// <param name="directive">The directive name, e.g. default-src</param>
    /// <param name="clear">Clear all previously set sources for this directive before adding new sources</param>
    /// <remarks>If you need a directive without a value, use <see cref="AddCustomDirective(string,bool)" /> instead</remarks>
    /// <returns>A configured <see cref="CspDirectiveBuilder" /></returns>
    public CspDirectiveBuilder AddCustomDirectiveBuilder(string directive, bool clear = false)
    {
        return AddOrGetDirective($"{nameof(AddCustomDirectiveBuilder)}({directive})",
            () => RawCspBuilder.AddCustomDirectiveBuilder(directive), clear);
    }

    internal T? GetCachedDirective<T>(string name) where T : CspDirectiveBuilderBase
    {
        return (T?)_directiveCache.GetValueOrDefault(name);
    }

    private T AddOrGetDirective<T>(string name, Func<T> directiveFactory, bool clear = false)
        where T : CspDirectiveBuilderBase
    {
        if (!clear && _directiveCache.TryGetValue(name, out var cachedDirective))
        {
            return (T)cachedDirective;
        }

        var directive = directiveFactory();
        _directiveCache[name] = directive;
        return directive;
    }
}