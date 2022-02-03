using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace Brickmakers.AspSecurityHeaders.PermissionPolicyBuilderExtensions;

/// <summary>
///     Extensions to <see cref="PermissionsPolicyBuilder" /> to configure the <c>publickey-credentials-get</c> permission.
/// </summary>
public static class PublickeyCredentialsGetPermission
{
    /// <summary>
    ///     Adds the <c>publickey-credentials-get</c> permission to the Permission-Policy header.
    /// </summary>
    /// <param name="permissionsPolicyBuilder">A <see cref="PermissionsPolicyBuilder" /> to add the permission to.</param>
    /// <returns>The permissionsPolicyBuilder that was passed as this.</returns>
    public static PermissionsPolicyDirectiveBuilder AddPublickeyCredentialsGet(
        this PermissionsPolicyBuilder permissionsPolicyBuilder)
    {
        return permissionsPolicyBuilder.AddCustomFeature("publickey-credentials-get");
    }
}