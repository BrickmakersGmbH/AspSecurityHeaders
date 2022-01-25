using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace Brickmakers.AspSecurityHeaders.PermissionPolicyBuilderExtensions
{
    public static class DisplayCapturePermission
    {
        public static PermissionsPolicyDirectiveBuilder AddDisplayCapture(
            this PermissionsPolicyBuilder permissionsPolicyBuilder)
        {
            return permissionsPolicyBuilder.AddCustomFeature("display-capture");
        }
    }
}