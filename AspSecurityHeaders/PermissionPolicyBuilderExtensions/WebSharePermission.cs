using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders.PermissionPolicyBuilderExtensions
{
    public static class WebSharePermission
    {
        public static PermissionsPolicyDirectiveBuilder AddWebShare(this PermissionsPolicyBuilder permissionsPolicyBuilder)
        {
            return permissionsPolicyBuilder.AddCustomFeature("web-share");
        }
    }
}