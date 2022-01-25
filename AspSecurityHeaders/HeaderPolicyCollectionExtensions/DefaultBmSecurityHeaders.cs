using Microsoft.AspNetCore.Builder;

namespace Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions
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
        
        public static HeaderPolicyCollection AddDefaultBmApiSecurityHeaders(this HeaderPolicyCollection headerPolicyCollection)
        {
            return headerPolicyCollection
                .AddContentTypeOptionsNoSniff()
                .AddStrictTransportSecurityMaxAgeIncludeSubDomains()
                .AddXPermittedCrossDomainPoliciesNone()
                .AddCacheControlNoStore()
                .RemoveServerHeader();
        }
    }
}