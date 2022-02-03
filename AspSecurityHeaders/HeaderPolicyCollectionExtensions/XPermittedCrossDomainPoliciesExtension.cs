using Microsoft.AspNetCore.Builder;

namespace Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;

/// <summary>
///     Extensions to <see cref="HeaderPolicyCollection" /> to configure the X-Permitted-Cross-Domain-Policies header.
/// </summary>
public static class XPermittedCrossDomainPoliciesExtension
{
    private const string Header = "X-Permitted-Cross-Domain-Policies";

    /// <summary>
    ///     Sets the <c>X-Permitted-Cross-Domain-Policies: none</c> header
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the header to.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    public static HeaderPolicyCollection AddXPermittedCrossDomainPoliciesNone(
        this HeaderPolicyCollection headerPolicyCollection)
    {
        return headerPolicyCollection.AddCustomHeader(Header, "none");
    }

    /// <summary>
    ///     Sets the <c>X-Permitted-Cross-Domain-Policies: master-only</c> header
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the header to.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    public static HeaderPolicyCollection AddXPermittedCrossDomainPoliciesMasterOnly(
        this HeaderPolicyCollection headerPolicyCollection)
    {
        return headerPolicyCollection.AddCustomHeader(Header, "master-only");
    }

    /// <summary>
    ///     Sets the <c>X-Permitted-Cross-Domain-Policies: by-content-type</c> header
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the header to.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    public static HeaderPolicyCollection AddXPermittedCrossDomainPoliciesByContentType(
        this HeaderPolicyCollection headerPolicyCollection)
    {
        return headerPolicyCollection.AddCustomHeader(Header, "by-content-type");
    }

    /// <summary>
    ///     Sets the <c>X-Permitted-Cross-Domain-Policies: all</c> header
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the header to.</param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    public static HeaderPolicyCollection AddXPermittedCrossDomainPoliciesAll(
        this HeaderPolicyCollection headerPolicyCollection)
    {
        return headerPolicyCollection.AddCustomHeader(Header, "all");
    }
}