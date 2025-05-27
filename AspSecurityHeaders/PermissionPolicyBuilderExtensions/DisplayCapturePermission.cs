using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace Brickmakers.AspSecurityHeaders.PermissionPolicyBuilderExtensions;

/// <summary>
///     Extensions to <see cref="PermissionsPolicyBuilder" /> to configure the <c>display-capture</c> permission.
/// </summary>
public static class DisplayCapturePermission
{
    /// <summary>
    ///     Adds the <c>display-capture</c> permission to the Permission-Policy header.
    /// </summary>
    /// <param name="permissionsPolicyBuilder">A <see cref="PermissionsPolicyBuilder" /> to add the permission to.</param>
    /// <returns>The permissionsPolicyBuilder that was passed as this.</returns>
    public static PermissionsPolicyDirectiveBuilder AddDisplayCapture(
        this PermissionsPolicyBuilder permissionsPolicyBuilder
    )
    {
        return permissionsPolicyBuilder.AddCustomFeature("display-capture");
    }
}
