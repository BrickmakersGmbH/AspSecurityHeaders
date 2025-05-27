using System.Text;
using FluentAssertions;
using Xunit;

namespace Brickmakers.AspSecurityHeaders.Generators.Test;

// ReSharper disable once InconsistentNaming
public class IISWebConfigWriterTests
{
    [Fact]
    public async Task GeneratesEmptyWebConfigByDefault()
    {
        var stringBuilder = new StringBuilder();

        await CreateDisabledWriter().Run(stringBuilder);

        stringBuilder
            .ToString()
            .Should()
            .Be(
                @"<?xml version=""1.0"" encoding=""utf-16""?>
<configuration>
  <system.webServer>
    <httpProtocol>
      <customHeaders />
    </httpProtocol>
  </system.webServer>
</configuration>"
            );
    }

    [Fact]
    public async Task GeneratesCustomFormattedXmlIfXmlWriterSettingsAreModified()
    {
        var stringBuilder = new StringBuilder();

        await CreateDisabledWriter()
            .SetXmlWriterSettings(settings => settings.OmitXmlDeclaration = true)
            .Run(stringBuilder);

        stringBuilder
            .ToString()
            .Should()
            .Be(
                @"<configuration>
  <system.webServer>
    <httpProtocol>
      <customHeaders />
    </httpProtocol>
  </system.webServer>
</configuration>"
            );
    }

    [Fact]
    public async Task GeneratesWebConfigWithRemovedServerHeadersIfRemoveServerHeadersIsSet()
    {
        var stringBuilder = new StringBuilder();

        await CreateDisabledWriter().RemoveServerHeaders(true).Run(stringBuilder);

        stringBuilder
            .ToString()
            .Should()
            .Be(
                @"<?xml version=""1.0"" encoding=""utf-16""?>
<configuration>
  <system.web>
    <httpRuntime enableVersionHeader=""false"" />
  </system.web>
  <system.webServer>
    <security>
      <requestFiltering removeServerHeader=""true"" />
    </security>
    <httpProtocol>
      <customHeaders>
        <remove name=""X-Powered-By"" />
      </customHeaders>
    </httpProtocol>
  </system.webServer>
</configuration>"
            );
    }

    [Fact]
    public async Task GeneratesWebConfigWithHttpsRedirectIfEnforceHttpsIsSet()
    {
        var stringBuilder = new StringBuilder();

        await CreateDisabledWriter().EnforceHttps(true).Run(stringBuilder);

        stringBuilder
            .ToString()
            .Should()
            .Be(
                @"<?xml version=""1.0"" encoding=""utf-16""?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name=""Enforce HTTPS"" enabled=""true"">
          <match url=""(.*)"" ignoreCase=""false"" />
          <conditions>
            <add input=""{HTTPS}"" pattern=""off"" />
          </conditions>
          <action type=""Redirect"" url=""https://{HTTP_HOST}/{R:1}"" appendQueryString=""false"" />
        </rule>
      </rules>
    </rewrite>
    <httpProtocol>
      <customHeaders />
    </httpProtocol>
  </system.webServer>
</configuration>"
            );
    }

    [Fact]
    public async Task GeneratesWebConfigWithAllDefaultSecurityHeadersIfDefaultSecurityHeadersConfigIsUsed()
    {
        var stringBuilder = new StringBuilder();

        await CreateSecurityHeadersOnlyWriter().Run(stringBuilder);

        stringBuilder
            .ToString()
            .Should()
            .Be(
                @"<?xml version=""1.0"" encoding=""utf-16""?>
<configuration>
  <system.webServer>
    <httpProtocol>
      <customHeaders>
        <add name=""X-Frame-Options"" value=""DENY"" />
        <add name=""X-Content-Type-Options"" value=""nosniff"" />
        <add name=""Strict-Transport-Security"" value=""max-age=31536000; includeSubDomains"" />
        <add name=""Referrer-Policy"" value=""no-referrer"" />
        <add name=""Content-Security-Policy"" value=""default-src 'none'; base-uri 'none'; form-action 'none'; frame-ancestors 'none'; script-src 'report-sample'; style-src 'report-sample'; upgrade-insecure-requests; block-all-mixed-content"" />
        <add name=""Cross-Origin-Opener-Policy"" value=""same-origin"" />
        <add name=""Cross-Origin-Embedder-Policy"" value=""require-corp"" />
        <add name=""Cross-Origin-Resource-Policy"" value=""same-origin"" />
        <add name=""X-Permitted-Cross-Domain-Policies"" value=""none"" />
        <add name=""Cache-Control"" value=""no-store"" />
        <add name=""Permissions-Policy"" value=""accelerometer=(), autoplay=self, camera=(), display-capture=(), encrypted-media=(), fullscreen=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), midi=(), payment=(), picture-in-picture=(), publickey-credentials-get=(), screen-wake-lock=(), sync-xhr=(), usb=(), web-share=(), xr-spatial-tracking=(), ambient-light-sensor=(), clipboard-read=(), clipboard-write=(), document-domain=(), interest-cohort=(), speaker=(), vr=()"" />
        <remove name=""Server"" />
        <remove name=""X-Powered-By"" />
      </customHeaders>
    </httpProtocol>
  </system.webServer>
</configuration>"
            );
    }

    [Fact]
    public async Task GeneratesWebConfigWithoutTlsHeadersIfWriteTlsHeadersIsFalse()
    {
        var stringBuilder = new StringBuilder();

        await CreateSecurityHeadersOnlyWriter().WriteHttpsHeaders(false).Run(stringBuilder);

        stringBuilder
            .ToString()
            .Should()
            .Be(
                @"<?xml version=""1.0"" encoding=""utf-16""?>
<configuration>
  <system.webServer>
    <httpProtocol>
      <customHeaders>
        <add name=""X-Frame-Options"" value=""DENY"" />
        <add name=""X-Content-Type-Options"" value=""nosniff"" />
        <add name=""Referrer-Policy"" value=""no-referrer"" />
        <add name=""Content-Security-Policy"" value=""default-src 'none'; base-uri 'none'; form-action 'none'; frame-ancestors 'none'; script-src 'report-sample'; style-src 'report-sample'; upgrade-insecure-requests; block-all-mixed-content"" />
        <add name=""Cross-Origin-Opener-Policy"" value=""same-origin"" />
        <add name=""Cross-Origin-Embedder-Policy"" value=""require-corp"" />
        <add name=""Cross-Origin-Resource-Policy"" value=""same-origin"" />
        <add name=""X-Permitted-Cross-Domain-Policies"" value=""none"" />
        <add name=""Cache-Control"" value=""no-store"" />
        <add name=""Permissions-Policy"" value=""accelerometer=(), autoplay=self, camera=(), display-capture=(), encrypted-media=(), fullscreen=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), midi=(), payment=(), picture-in-picture=(), publickey-credentials-get=(), screen-wake-lock=(), sync-xhr=(), usb=(), web-share=(), xr-spatial-tracking=(), ambient-light-sensor=(), clipboard-read=(), clipboard-write=(), document-domain=(), interest-cohort=(), speaker=(), vr=()"" />
        <remove name=""Server"" />
        <remove name=""X-Powered-By"" />
      </customHeaders>
    </httpProtocol>
  </system.webServer>
</configuration>"
            );
    }

    private static IISWebConfigWriter CreateSecurityHeadersOnlyWriter()
    {
        return IISWebConfigWriter.Create().RemoveServerHeaders(false).EnforceHttps(false);
    }

    private static IISWebConfigWriter CreateDisabledWriter()
    {
        return CreateSecurityHeadersOnlyWriter()
            .SetBmSecurityHeadersConfig(new BmSecurityHeadersConfig())
            .WriteHttpsHeaders(false);
    }
}
