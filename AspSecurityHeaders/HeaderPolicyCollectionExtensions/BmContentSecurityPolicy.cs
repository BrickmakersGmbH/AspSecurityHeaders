using System;
using Microsoft.AspNetCore.Builder;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders.HeaderPolicyCollectionExtensions
{
    public static class BmContentSecurityPolicy
    {
        public static HeaderPolicyCollection AddBmContentSecurityPolicy(
            this HeaderPolicyCollection headerPolicyCollection, Action<CspBuilder> cspBuilder, 
            bool allowInsecureRequests = false, bool allowMixedContent = false)
        {
            return headerPolicyCollection.AddContentSecurityPolicy(builder =>
            {
                builder.AddDefaultSrc().None();
                builder.AddBaseUri().None();
                builder.AddFormAction().None();
                builder.AddFrameAncestors().None();
                if (!allowInsecureRequests)
                {
                    builder.AddUpgradeInsecureRequests();
                }
                if (!allowMixedContent)
                {
                    builder.AddBlockAllMixedContent();
                }
                cspBuilder(builder);
            });
        }
    }
}