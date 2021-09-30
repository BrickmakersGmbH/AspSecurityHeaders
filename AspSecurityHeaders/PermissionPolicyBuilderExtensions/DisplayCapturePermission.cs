using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders.PermissionPolicyBuilderExtensions
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