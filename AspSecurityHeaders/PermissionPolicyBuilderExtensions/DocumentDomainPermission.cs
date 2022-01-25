using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers.PermissionsPolicy;

namespace Brickmakers.AspSecurityHeaders.PermissionPolicyBuilderExtensions;

public static class DocumentDomainPermission
{
    public static PermissionsPolicyDirectiveBuilder AddDocumentDomain(
        this PermissionsPolicyBuilder permissionsPolicyBuilder)
    {
        return permissionsPolicyBuilder.AddCustomFeature("document-domain");
    }
}