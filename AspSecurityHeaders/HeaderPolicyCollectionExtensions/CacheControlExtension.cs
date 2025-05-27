using Microsoft.AspNetCore.Builder;

namespace Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;

/// <summary>
///     Extensions to <see cref="HeaderPolicyCollection" /> to configure the Cache-Control header.
/// </summary>
public static class CacheControlExtension
{
    private const string Header = "Cache-Control";

    /// <summary>
    ///     Sets the <c>Cache-Control: private</c> header
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the header to.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    public static HeaderPolicyCollection AddCacheControlPrivate(
        this HeaderPolicyCollection headerPolicyCollection
    )
    {
        return headerPolicyCollection.AddCacheControl("private");
    }

    /// <summary>
    ///     Sets the <c>Cache-Control: public</c> header
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the header to.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    public static HeaderPolicyCollection AddCacheControlPublic(
        this HeaderPolicyCollection headerPolicyCollection
    )
    {
        return headerPolicyCollection.AddCacheControl("public");
    }

    /// <summary>
    ///     Sets the <c>Cache-Control: no-cache</c> header
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the header to.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    public static HeaderPolicyCollection AddCacheControlNoCache(
        this HeaderPolicyCollection headerPolicyCollection
    )
    {
        return headerPolicyCollection.AddCacheControl("no-cache");
    }

    /// <summary>
    ///     Sets the <c>Cache-Control: no-store</c> header
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the header to.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    public static HeaderPolicyCollection AddCacheControlNoStore(
        this HeaderPolicyCollection headerPolicyCollection
    )
    {
        return headerPolicyCollection.AddCacheControl("no-store");
    }

    /// <summary>
    ///     Sets the <c>Cache-Control</c> header to a custom value.
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the header to.</param>
    /// <param name="cacheControl">The value of the cache control header. Can be a comma-seperated list of values.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    public static HeaderPolicyCollection AddCacheControl(
        this HeaderPolicyCollection headerPolicyCollection,
        string cacheControl
    )
    {
        return headerPolicyCollection.AddCustomHeader(Header, cacheControl);
    }

    /// <summary>
    ///     Removes any configuration for the <c>Cache-Control</c> header, include the default values.
    ///     This may be needed in case cache control should be configured by some other mechanism.
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to remove the configuration from.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    public static HeaderPolicyCollection UnconfigureCacheControl(
        this HeaderPolicyCollection headerPolicyCollection
    )
    {
        headerPolicyCollection.Remove(Header);
        return headerPolicyCollection;
    }
}
