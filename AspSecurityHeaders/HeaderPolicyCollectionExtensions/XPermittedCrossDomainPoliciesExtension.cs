using Microsoft.AspNetCore.Builder;

namespace Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions
{
    public static class XPermittedCrossDomainPoliciesExtension
    {
        private const string Header = "X-Permitted-Cross-Domain-Policies";
        
        public static HeaderPolicyCollection AddXPermittedCrossDomainPoliciesNone(
            this HeaderPolicyCollection headerPolicyCollection)
        {
            return headerPolicyCollection.AddCustomHeader(Header, "none");
        }
        
        public static HeaderPolicyCollection AddXPermittedCrossDomainPoliciesMasterOnly(
            this HeaderPolicyCollection headerPolicyCollection)
        {
            return headerPolicyCollection.AddCustomHeader(Header, "master-only");
        }
        
        public static HeaderPolicyCollection AddXPermittedCrossDomainPoliciesByContentType(
            this HeaderPolicyCollection headerPolicyCollection)
        {
            return headerPolicyCollection.AddCustomHeader(Header, "by-content-type");
        }
        
        public static HeaderPolicyCollection AddXPermittedCrossDomainPoliciesAll(
            this HeaderPolicyCollection headerPolicyCollection)
        {
            return headerPolicyCollection.AddCustomHeader(Header, "all");
        }
    }
}