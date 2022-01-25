using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace Brickmakers.AspSecurityHeaders.PermissionPolicyBuilderExtensions;

public static class XrSpatialTrackingPermission
{
    public static PermissionsPolicyDirectiveBuilder AddXrSpatialTracking(
        this PermissionsPolicyBuilder permissionsPolicyBuilder)
    {
        return permissionsPolicyBuilder.AddCustomFeature("xr-spatial-tracking");
    }
}