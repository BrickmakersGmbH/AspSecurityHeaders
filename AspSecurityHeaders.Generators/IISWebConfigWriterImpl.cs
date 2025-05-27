using System.Xml;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NetEscapades.AspNetCore.SecurityHeaders.Headers;
using NetEscapades.AspNetCore.SecurityHeaders.Infrastructure;

namespace Brickmakers.AspSecurityHeaders.Generators;

// ReSharper disable once InconsistentNaming
internal class IISWebConfigWriterImpl : IDisposable, IAsyncDisposable
{
    private readonly IISWebConfigWriterSettings _settings;
    private readonly XmlWriter _writer;

    internal IISWebConfigWriterImpl(XmlWriter writer, IISWebConfigWriterSettings settings)
    {
        _writer = writer;
        _settings = settings;
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        _writer.Dispose();
        return new ValueTask();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _writer.Dispose();
    }

    internal async Task Run()
    {
        await _writer.WriteStartDocumentAsync();
        await _writer.WriteStartElementAsync("configuration");

        if (_settings.RemoveServerHeaders)
        {
            await WriteSystemWeb();
        }

        await WriteSystemWebServer();

        await _writer.WriteEndElementAsync();
        await _writer.FlushAsync();
    }

    private async Task WriteSystemWeb()
    {
        await _writer.WriteStartElementAsync("system.web");
        await _writer.WriteStartElementAsync("httpRuntime");
        await _writer.WriteAttributeAsync("enableVersionHeader", false);
        await _writer.WriteEndElementAsync();
        await _writer.WriteEndElementAsync();
    }

    private async Task WriteSystemWebServer()
    {
        await _writer.WriteStartElementAsync("system.webServer");

        if (_settings.RemoveServerHeaders)
        {
            await WriteSecurity();
        }

        if (_settings.EnforceHttps)
        {
            await WriteRewrite();
        }

        await WriteHttpProtocol();

        await _writer.WriteEndElementAsync();
    }

    private async Task WriteSecurity()
    {
        await _writer.WriteStartElementAsync("security");
        await _writer.WriteStartElementAsync("requestFiltering");
        await _writer.WriteAttributeAsync("removeServerHeader", true);
        await _writer.WriteEndElementAsync();
        await _writer.WriteEndElementAsync();
    }

    private async Task WriteRewrite()
    {
        await _writer.WriteStartElementAsync("rewrite");
        await _writer.WriteStartElementAsync("rules");
        await _writer.WriteStartElementAsync("rule");
        await _writer.WriteAttributeAsync("name", "Enforce HTTPS");
        await _writer.WriteAttributeAsync("enabled", true);

        await _writer.WriteStartElementAsync("match");
        await _writer.WriteAttributeAsync("url", "(.*)");
        await _writer.WriteAttributeAsync("ignoreCase", false);
        await _writer.WriteEndElementAsync();

        await _writer.WriteStartElementAsync("conditions");
        await _writer.WriteStartElementAsync("add");
        await _writer.WriteAttributeAsync("input", "{HTTPS}");
        await _writer.WriteAttributeAsync("pattern", "off");
        await _writer.WriteEndElementAsync();
        await _writer.WriteEndElementAsync();

        await _writer.WriteStartElementAsync("action");
        await _writer.WriteAttributeAsync("type", "Redirect");
        await _writer.WriteAttributeAsync("url", "https://{HTTP_HOST}/{R:1}");
        await _writer.WriteAttributeAsync("appendQueryString", false);
        await _writer.WriteEndElementAsync();

        await _writer.WriteEndElementAsync();
        await _writer.WriteEndElementAsync();
        await _writer.WriteEndElementAsync();
    }

    private async Task WriteHttpProtocol()
    {
        await _writer.WriteStartElementAsync("httpProtocol");
        await _writer.WriteStartElementAsync("customHeaders");

        await WriteSecurityHeaders();

        if (_settings.RemoveServerHeaders)
        {
            await WriteRemoveXPoweredBy();
        }

        await _writer.WriteEndElementAsync();
        await _writer.WriteEndElementAsync();
    }

    private async Task WriteRemoveXPoweredBy()
    {
        await _writer.WriteStartElementAsync("remove");
        await _writer.WriteAttributeAsync("name", "X-Powered-By");
        await _writer.WriteEndElementAsync();
    }

    private async Task WriteSecurityHeaders()
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var fakeContext = new DefaultHttpContext();
        fakeContext.Request.IsHttps = _settings.WriteHttpsHeaders;
        var headersResult = new CustomHeadersResult();

        foreach (var policy in _settings.BmSecurityHeadersConfig.Values)
        {
            policy.Apply(fakeContext, headersResult);
        }

        foreach (var (name, value) in headersResult.SetHeaders)
        {
            await _writer.WriteStartElementAsync("add");
            await _writer.WriteAttributeAsync("name", name);
            await _writer.WriteAttributeAsync("value", value);
            await _writer.WriteEndElementAsync();
        }

        foreach (var name in headersResult.RemoveHeaders)
        {
            await _writer.WriteStartElementAsync("remove");
            await _writer.WriteAttributeAsync("name", name);
            await _writer.WriteEndElementAsync();
        }
    }
}
