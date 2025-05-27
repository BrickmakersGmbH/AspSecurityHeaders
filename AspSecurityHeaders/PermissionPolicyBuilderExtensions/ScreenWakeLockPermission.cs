using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace Brickmakers.AspSecurityHeaders.PermissionPolicyBuilderExtensions;

/// <summary>
///     Extensions to <see cref="PermissionsPolicyBuilder" /> to configure the <c>screen-wake-lock</c> permission.
/// </summary>
public static class ScreenWakeLockPermission
{
    /// <summary>
    ///     Adds the <c>screen-wake-lock</c> permission to the Permission-Policy header.
    /// </summary>
    /// <param name="permissionsPolicyBuilder">A <see cref="PermissionsPolicyBuilder" /> to add the permission to.</param>
    /// <returns>The permissionsPolicyBuilder that was passed as this.</returns>
    public static PermissionsPolicyDirectiveBuilder AddScreenWakeLock(
        this PermissionsPolicyBuilder permissionsPolicyBuilder
    )
    {
        return permissionsPolicyBuilder.AddCustomFeature("screen-wake-lock");
    }
}
