using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace Brickmakers.AspSecurityHeaders.PermissionPolicyBuilderExtensions
{
    public static class ClipboardPermissions
    {
        public static PermissionsPolicyDirectiveBuilder AddClipboardRead(this PermissionsPolicyBuilder permissionsPolicyBuilder)
        {
            return permissionsPolicyBuilder.AddCustomFeature("clipboard-read");
        }
        
        public static PermissionsPolicyDirectiveBuilder AddClipboardWrite(this PermissionsPolicyBuilder permissionsPolicyBuilder)
        {
            return permissionsPolicyBuilder.AddCustomFeature("clipboard-write");
        }
    }
}