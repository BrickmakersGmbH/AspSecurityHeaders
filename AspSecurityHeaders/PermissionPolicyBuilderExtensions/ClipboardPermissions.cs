using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace Brickmakers.AspSecurityHeaders.PermissionPolicyBuilderExtensions;

/// <summary>
///     Extensions to <see cref="PermissionsPolicyBuilder" /> to add clipboard permissions
/// </summary>
public static class ClipboardPermissions
{
    /// <summary>
    ///     Adds the <c>clipboard-read</c> permission to the Permission-Policy header.
    /// </summary>
    /// <param name="permissionsPolicyBuilder">A <see cref="PermissionsPolicyBuilder" /> to add the permission to.</param>
    /// <returns>The permissionsPolicyBuilder that was passed as this.</returns>
    public static PermissionsPolicyDirectiveBuilder AddClipboardRead(
        this PermissionsPolicyBuilder permissionsPolicyBuilder
    )
    {
        return permissionsPolicyBuilder.AddCustomFeature("clipboard-read");
    }

    /// <summary>
    ///     Adds the <c>clipboard-write</c> permission to the Permission-Policy header.
    /// </summary>
    /// <param name="permissionsPolicyBuilder">A <see cref="PermissionsPolicyBuilder" /> to add the permission to.</param>
    /// <returns>The permissionsPolicyBuilder that was passed as this.</returns>
    public static PermissionsPolicyDirectiveBuilder AddClipboardWrite(
        this PermissionsPolicyBuilder permissionsPolicyBuilder
    )
    {
        return permissionsPolicyBuilder.AddCustomFeature("clipboard-write");
    }
}
