using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace Brickmakers.AspSecurityHeaders.PermissionPolicyBuilderExtensions
{
    public static class PublickeyCredentialsGetPermission
    {
        public static PermissionsPolicyDirectiveBuilder AddPublickeyCredentialsGet(this PermissionsPolicyBuilder permissionsPolicyBuilder)
        {
            return permissionsPolicyBuilder.AddCustomFeature("publickey-credentials-get");
        }
    }
}