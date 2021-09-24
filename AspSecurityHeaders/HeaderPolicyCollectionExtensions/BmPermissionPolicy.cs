using System;
using System.Linq;
using de.brickmakers.SecurityEngineering.AspSecurityHeaders.HeaderPolicies;
using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders.HeaderPolicyCollectionExtensions
{
    public static class BmPermissionPolicy
    {
        public static HeaderPolicyCollection AddBmPermissionPolicy(this HeaderPolicyCollection headerPolicyCollection,
            Action<PermissionsPolicyBuilder> configure,
            bool addFeaturePolicy = true)
        {
            headerPolicyCollection.AddPermissionsPolicy(builder =>
            {
                configure(builder);
            });
            
            // ReSharper disable once InvertIf
            if (addFeaturePolicy)
            {
                var permissionPolicy = headerPolicyCollection.Values.First(policy => policy is PermissionsPolicyHeader);
                var bmFeaturePolicy = new BmFeaturePolicyHeader(permissionPolicy);
                headerPolicyCollection.Add(bmFeaturePolicy.Header, bmFeaturePolicy);
            }
            
            return headerPolicyCollection;
        }
    }
}