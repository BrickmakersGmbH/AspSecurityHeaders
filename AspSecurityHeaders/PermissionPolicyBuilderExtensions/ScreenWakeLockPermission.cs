using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders.PermissionPolicyBuilderExtensions
{
    public static class ScreenWakeLockPermission
    {
        public static PermissionsPolicyDirectiveBuilder AddScreenWakeLock(this PermissionsPolicyBuilder permissionsPolicyBuilder)
        {
            return permissionsPolicyBuilder.AddCustomFeature("screen-wake-lock");
        }
    }
}