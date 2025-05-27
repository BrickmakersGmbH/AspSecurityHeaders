using Brickmakers.AspSecurityHeaders.PermissionPolicyBuilderExtensions;
using Microsoft.AspNetCore.Builder;

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
    /// <returns>The headerPolicyCollection that was passed as this.</returns>
    /// <remarks>
    ///     You can easily overwrite any of the default policies by calling <c>builder.AddXXX()</c> again. For example,
    ///     to use a custom camera permission, use:
    ///     <code>
    ///         builder => builder.AddCamera().Self();
    ///     </code>
    /// </remarks>
    public static HeaderPolicyCollection AddBmPermissionPolicy(
        this HeaderPolicyCollection headerPolicyCollection,
        Action<PermissionsPolicyBuilder> configure
    )
    {
        headerPolicyCollection.AddPermissionsPolicy(builder =>
        {
            builder.AddDefaultSecureDirectives();
            builder.AddAmbientLightSensor().None();
            builder.AddClipboardRead().None();
            builder.AddClipboardWrite().None();
            builder.AddDocumentDomain().None();
            builder.AddFederatedLearningOfCohortsCalculation().None();
            builder.AddSpeaker().None();
            builder.AddVR().None();

            builder.AddAutoplay().Self();

            configure(builder);
        });

        return headerPolicyCollection;
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Obsolete(
        "The 'addFeaturePolicy' parameter is no longer used because the FeaturePolicy header has been deprecated and will be removed in a future version. Use the overload without it."
    )]
    public static HeaderPolicyCollection AddBmPermissionPolicy(
        this HeaderPolicyCollection headerPolicyCollection,
        Action<PermissionsPolicyBuilder> configure,
        bool addFeaturePolicy
    )
    {
        return headerPolicyCollection.AddBmPermissionPolicy(configure);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
