using Microsoft.AspNetCore.Builder;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders.HeaderPolicyCollectionExtensions
{
    public static class DefaultBmSecurityHeaders
    {
        public static HeaderPolicyCollection AddDefaultBmSecurityHeaders(this HeaderPolicyCollection headerPolicyCollection)
        {
            return headerPolicyCollection
                .AddDefaultSecurityHeaders()
                .AddStrictTransportSecurityMaxAgeIncludeSubDomains()
                .AddXssProtectionDisabled()
                .AddReferrerPolicyNoReferrer()
                .AddXPermittedCrossDomainPoliciesNone()
                .AddCacheControlNoStore()
                .AddBmContentSecurityPolicy(builder => {})
                .AddBmPermissionPolicy(builder => {})
                .RemoveServerHeader();
        }
    }
}