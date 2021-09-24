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
                builder.AddAccelerometer().None();
                builder.AddAmbientLightSensor().None();
                builder.AddCamera().None();
                builder.AddCustomFeature("clipboard-read").None();
                builder.AddCustomFeature("clipboard-write").None();
                builder.AddCustomFeature("display-capture").None();
                builder.AddCustomFeature("document-domain").None();
                builder.AddEncryptedMedia().None();
                builder.AddFederatedLearningOfCohortsCalculation().None();
                builder.AddGeolocation().None();
                builder.AddGyroscope().None();
                builder.AddMagnetometer().None();
                builder.AddMicrophone().None();
                builder.AddMidi().None();
                builder.AddPayment().None();
                builder.AddCustomFeature("publickey-credentials-get").None();
                builder.AddCustomFeature("screen-wake-lock").None();
                builder.AddSpeaker().None();
                builder.AddUsb().None();
                builder.AddVR().None();
                builder.AddCustomFeature("web-share").None();
                builder.AddCustomFeature("xr-spatial-tracking").None();
                
                builder.AddAutoplay().Self();
                builder.AddFullscreen().Self();
                builder.AddPictureInPicture().Self();
                builder.AddSyncXHR().Self();
                
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