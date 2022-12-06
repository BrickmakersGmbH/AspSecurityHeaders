using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Brickmakers.AspSecurityHeaders.Controllers.Models;
using Brickmakers.AspSecurityHeaders.Test;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Brickmakers.AspSecurityHeaders.OrchardModule.Test;

public class OrchardModuleTest : IClassFixture<WebApplicationFactory<Example.Startup>>
{
    private readonly WebApplicationFactory<Example.Startup> _factory;

    public OrchardModuleTest(WebApplicationFactory<Example.Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ShouldReturnNotFoundResponseWithOrchardSecurityHeaders()
    {
        var response = await GetIndex();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // ensure a security header normally not present is set
        response.Headers.Should().ContainKey("Cross-Origin-Embedder-Policy");
        // ensure a normally present header was removed
        response.Headers.Should().NotContainKey("Server");
    }

    [Theory]
    [InlineData("default-src", new[] {"'none'"})]
    [InlineData("base-uri", new[] {"'none'"})]
    [InlineData("frame-ancestors", new[] {"'none'"})]
    [InlineData("script-src", new[]
    {
        "'self'",
        "'unsafe-inline'",
        "'unsafe-eval'",
        "'report-sample'"
    })]
    [InlineData("style-src", new[]
    {
        "'self'",
        "https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@6.2.0/css/",
        "'unsafe-inline'",
        "'report-sample'"
    })]
    [InlineData("img-src", new[] {"'self'", "data:"})]
    [InlineData("font-src", new[]
    {
        "'self'",
        "https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@6.2.0/webfonts/"
    })]
    [InlineData("form-action", new[] {"'self'"})]
    [InlineData("connect-src", new[] {"'self'"})]
    [InlineData("upgrade-insecure-requests", new string[0])]
    [InlineData("block-all-mixed-content", new string[0])]
    [InlineData("report-uri", new[] {"/CspReport"})]
    public async Task ShouldContainCspValues(string cspName, string[] cspValues)
    {
        var response = await GetIndex();

        response.Headers.Should().ContainKey("Content-Security-Policy");
        var csp = HeaderExtractor.ParseCsp(response.Headers.GetValues("Content-Security-Policy"));
        csp.Should().ContainKey(cspName);
        csp[cspName].Should().BeEquivalentTo(cspValues);
    }

    [Fact]
    public async Task ShouldAcceptCspReports()
    {
        using var client = _factory.CreateClient();
        var response = await client.PostAsync("/CspReport", JsonContent.Create(
            new CspReportRequest
            {
                CspReport = new CspReport
                {
                    StatusCode = 400,
                    BlockedUri = "http://localhost/"
                }
            },
            new MediaTypeHeaderValue("application/csp-report")));

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task<HttpResponseMessage> GetIndex()
    {
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/");
        return response;
    }
}