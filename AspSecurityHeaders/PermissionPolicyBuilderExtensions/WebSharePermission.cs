using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace Brickmakers.AspSecurityHeaders.PermissionPolicyBuilderExtensions;

/// <summary>
///     Extensions to <see cref="PermissionsPolicyBuilder" /> to configure the <c>web-share</c> permission.
/// </summary>
public static class WebSharePermission
{
    /// <summary>
    ///     Adds the <c>web-share</c> permission to the Permission-Policy header.
    /// </summary>
    /// <param name="permissionsPolicyBuilder">A <see cref="PermissionsPolicyBuilder" /> to add the permission to.</param>
    /// <returns>The permissionsPolicyBuilder that was passed as this.</returns>
    public static PermissionsPolicyDirectiveBuilder AddWebShare(this PermissionsPolicyBuilder permissionsPolicyBuilder)
    {
        return permissionsPolicyBuilder.AddCustomFeature("web-share");
    }
}