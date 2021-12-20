using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace AspSecurityHeaders.Test
{
    public class SecurityHeadersTests : IClassFixture<WebApplicationFactory<Example.Startup>>
    {
        private readonly WebApplicationFactory<Example.Startup> _factory;
        private readonly ITestOutputHelper _output;

        public SecurityHeadersTests(WebApplicationFactory<Example.Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public async Task ShouldReturnValidResponse()
        {
            var response = await GetIndex();

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("X-Frame-Options", "DENY")]
        [InlineData("X-XSS-Protection", "0")]
        [InlineData("X-Content-Type-Options", "nosniff")]
        [InlineData("Referrer-Policy", "no-referrer")]
        [InlineData("X-Permitted-Cross-Domain-Policies", "none")]
        [InlineData("Cache-Control", "no-store")]
        public async Task ShouldContainStaticSecurityHeaders(string headerName, string headerValue)
        {
            var response = await GetIndex();
            response.Headers.Should().ContainKey(headerName);
            var headerValues = response.Headers.GetValues(headerName);
            headerValues.Should().ContainSingle(headerValue);
        }

        [Fact]
        public async Task ShouldNotContainerServerHeader()
        {
            var response = await GetIndex();
            response.Headers.Should().NotContainKey("Server");
        }

        [Theory]
        [InlineData("accelerometer", null)]
        [InlineData("ambient-light-sensor", null)]
        [InlineData("camera", null)]
        [InlineData("clipboard-read", null)]
        [InlineData("clipboard-write", null)]
        [InlineData("display-capture", null)]
        [InlineData("document-domain", null)]
        [InlineData("encrypted-media", null)]
        [InlineData("interest-cohort", null)]
        [InlineData("geolocation", null)]
        [InlineData("gyroscope", null)]
        [InlineData("magnetometer", null)]
        [InlineData("microphone", null)]
        [InlineData("midi", null)]
        [InlineData("payment", null)]
        [InlineData("publickey-credentials-get", null)]
        [InlineData("screen-wake-lock", null)]
        [InlineData("speaker", null)]
        [InlineData("usb", null)]
        [InlineData("vr", null)]
        [InlineData("web-share", null)]
        [InlineData("xr-spatial-tracking", null)]
        [InlineData("autoplay", "self")]
        [InlineData("fullscreen", "self")]
        [InlineData("picture-in-picture", "self")]
        [InlineData("sync-xhr", "self")]
        public async Task ShouldContainPermissionAndFeaturePolicyValues(string name, string? expectedValue)
        {
            var response = await GetIndex();
            
            response.Headers.Should().ContainKey("Permissions-Policy");
            var permissions = ParsePermissions(response.Headers.GetValues("Permissions-Policy"));
            permissions.Should().ContainKey(name);
            var permissionValue = permissions[name];
            
            response.Headers.Should().ContainKey("Feature-Policy");
            var features = ParseFeatures(response.Headers.GetValues("Feature-Policy"));
            features.Should().ContainKey(name);
            var featureValue = features[name];
            
            if (expectedValue == null)
            {
                permissionValue.Should().Be("()");
                featureValue.Should().Be("'none'");
            }
            else
            {
                permissionValue.Should().Be(expectedValue);
                featureValue.Should().Be($"'{expectedValue}'");
            }
        }

        [Theory]
        [InlineData("default-src", new [] { "'none'" })]
        [InlineData("base-uri", new [] { "'none'" })]
        [InlineData("form-action", new [] { "'none'" })]
        [InlineData("frame-ancestors", new [] { "'none'" })]
        [InlineData("script-src", new [] { "'self'", "'report-sample'" })]
        [InlineData("style-src", new [] { "'self'", "'report-sample'" })]
        [InlineData("img-src", new [] { "'self'" })]
        [InlineData("upgrade-insecure-requests", new string[0])]
        [InlineData("block-all-mixed-content", new string[0])]
        [InlineData("report-uri", new [] { "https://localhost:5001/CspReport" })]
        public async Task ShouldContainCspValues(string cspName, string[] cspValues)
        {
            var response = await GetIndex();
            
            response.Headers.Should().ContainKey("Content-Security-Policy");
            var csp = ParseCsp(response.Headers.GetValues("Content-Security-Policy"));
            csp.Should().ContainKey(cspName);
            csp[cspName].Should().BeEquivalentTo(cspValues);
        }

        private async Task<HttpResponseMessage> GetIndex()
        {
            using var client = _factory.CreateClient();
            var response = await client.GetAsync("/");
            _output.WriteLine(response.Headers.ToString());
            return response;
        }

        private static IEnumerable<string> ExtractPermissions(string permissions)
        {
            var parenthesisCount = 0;
            var begin = 0;
            for (var index = 0; index < permissions.Length; ++index)
            {
                switch (permissions[index])
                {
                    case '(':
                        parenthesisCount++;
                        break;
                    case ')':
                        parenthesisCount--;
                        break;
                    case ',':
                        if (parenthesisCount == 0)
                        {
                            yield return permissions[begin..index]
                                .Trim();
                            begin = index + 1;
                        }
                        break;
                }
            }

            yield return permissions[begin..].Trim();
        }

        private static KeyValuePair<string, string> ParsePermission(string permission)
        {
            var equalsIndex = permission.IndexOf('=');
            var name = permission[..equalsIndex];
            var value = permission[(equalsIndex + 1)..];
            return new KeyValuePair<string, string>(name, value);
        }

        private static IReadOnlyDictionary<string, string> ParsePermissions(IEnumerable<string> permissions)
        {
            return new Dictionary<string, string>(permissions
                .SelectMany(ExtractPermissions)
                .Select(ParsePermission)
            );
        }

        private static KeyValuePair<string, string> ParseFeature(string feature)
        {
            var spaceIndex = feature.IndexOf(' ');
            var name = feature[..spaceIndex];
            var value = feature[(spaceIndex + 1)..];
            return new KeyValuePair<string, string>(name, value);
        }

        private static IReadOnlyDictionary<string, string> ParseFeatures(IEnumerable<string> features)
        {
            return new Dictionary<string, string>(features
                .SelectMany(featureValues => featureValues.Split(';'))
                .Select(feature => feature.Trim())
                .Where(feature => feature.Length > 0)
                .Select(ParseFeature)
            );
        }

        private static KeyValuePair<string, IEnumerable<string>> ParseCsp(string csp)
        {
            var values = csp.Trim().Split(' ');
            return new KeyValuePair<string, IEnumerable<string>>(values[0], values[1..]);
        }

        private static IReadOnlyDictionary<string, IEnumerable<string>> ParseCsp(IEnumerable<string> csp)
        {
            return new Dictionary<string, IEnumerable<string>>(csp
                .SelectMany(cspValues => cspValues.Split(';'))
                .Select(cspEntry => cspEntry.Trim())
                .Where(cspEntry => cspEntry.Length > 0)
                .Select(ParseCsp)
            );
        }
    }

}