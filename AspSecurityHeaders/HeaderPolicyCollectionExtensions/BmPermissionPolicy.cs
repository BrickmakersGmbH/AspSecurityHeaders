using Brickmakers.AspSecurityHeaders.HeaderPolicies;
using Brickmakers.AspSecurityHeaders.PermissionPolicyBuilderExtensions;
using Microsoft.AspNetCore.Builder;
using NetEscapades.AspNetCore.SecurityHeaders.Headers;

namespace Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;

/// <summary>
///     Extensions to <see cref="HeaderPolicyCollection" /> to configure the Permission Policy
/// </summary>
public static class BmPermissionPolicy
{
    /// <summary>
    ///     Adds a Permission-Policy-Header to the security headers with the default secure basis of denied permissions,
    ///     combined with however the policy is configured afterwards. The standard directives are:
    ///     <code>
    ///         accelerometer=(),
    ///         ambient-light-sensor=(),
    ///         camera=(),
    ///         clipboard-read=(),
    ///         clipboard-write=(),
    ///         display-capture=(),
    ///         document-domain=(),
    ///         encrypted-media=(),
    ///         interest-cohort=(),
    ///         geolocation=(),
    ///         gyroscope=(),
    ///         magnetometer=(),
    ///         microphone=(),
    ///         midi=(),
    ///         payment=(),
    ///         publickey-credentials-get=(),
    ///         screen-wake-lock=(),
    ///         speaker=(),
    ///         usb=(),
    ///         vr=(),
    ///         web-share=(),
    ///         xr-spatial-tracking=(),
    ///         autoplay=self,
    ///         fullscreen=self,
    ///         picture-in-picture=self,
    ///         sync-xhr=self
    ///     </code>
    /// </summary>
    /// <param name="headerPolicyCollection">A <see cref="HeaderPolicyCollection" /> to add the Permission-Policy to.</param>
    /// <param name="configure">
    ///     A configure callback that provides a <see cref="PermissionsPolicyBuilder" /> to add policies to. The build is
    ///     already preconfigured with a secure basis of policies.
    /// </param>
    /// <param name="addFeaturePolicy">
    ///     If set to true (the default), a <c>Feature-Policy</c> header will be added as well, with
    ///     the same values as for the permission policy. This is useful to support older browsers.
    /// </param>
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    /// <remarks>
    ///     You can easily overwrite any of the default policies by calling <c>builder.AddXXX()</c> again. For example,
    ///     to use a custom camera permission, use:
    ///     <code>
    ///         builder => builder.AddCamera().Self();
    ///     </code>
    /// </remarks>
    public static HeaderPolicyCollection AddBmPermissionPolicy(this HeaderPolicyCollection headerPolicyCollection,
        Action<PermissionsPolicyBuilder> configure,
        bool addFeaturePolicy = true)
    {
        headerPolicyCollection.AddPermissionsPolicy(builder =>
        {
            builder.AddAccelerometer().None();
            builder.AddAmbientLightSensor().None();
            builder.AddCamera().None();
            builder.AddClipboardRead().None();
            builder.AddClipboardWrite().None();
            builder.AddDisplayCapture().None();
            builder.AddDocumentDomain().None();
            builder.AddEncryptedMedia().None();
            builder.AddFederatedLearningOfCohortsCalculation().None();
            builder.AddGeolocation().None();
            builder.AddGyroscope().None();
            builder.AddMagnetometer().None();
            builder.AddMicrophone().None();
            builder.AddMidi().None();
            builder.AddPayment().None();
            builder.AddPublickeyCredentialsGet().None();
            builder.AddScreenWakeLock().None();
            builder.AddSpeaker().None();
            builder.AddUsb().None();
            builder.AddVR().None();
            builder.AddWebShare().None();
            builder.AddXrSpatialTracking().None();

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
            var bmFeaturePolicy = new BmFeaturePolicyHeader((PermissionsPolicyHeader) permissionPolicy);
            headerPolicyCollection.Add(bmFeaturePolicy.Header, bmFeaturePolicy);
        }

        return headerPolicyCollection;
    }
}