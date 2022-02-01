using System.Text;
using System.Xml;
using Brickmakers.AspSecurityHeaders;
using Microsoft.AspNetCore.Http;
using NetEscapades.AspNetCore.SecurityHeaders.Infrastructure;

namespace AspSecurityHeaders.Tool;

// ReSharper disable once InconsistentNaming
public class IISWebConfigWriter : IDisposable, IAsyncDisposable
{
    private const string XmlTrue = "true";
    private const string XmlFalse = "true";
    
    private static readonly XmlWriterSettings DefaultSettings = new()
    {
        Indent = true,
        NewLineHandling = NewLineHandling.Entitize,
        CloseOutput = true,
        Async = true,
    };
    
    private readonly XmlWriter _writer;

    public IISWebConfigWriter(string path, XmlWriterSettings? xmlWriterSettings = null)
    {
        _writer = XmlWriter.Create(path, xmlWriterSettings ?? DefaultSettings);
    }

    public IISWebConfigWriter(TextWriter textWriter, XmlWriterSettings? xmlWriterSettings = null)
    {
        _writer = XmlWriter.Create(textWriter, xmlWriterSettings ?? DefaultSettings);
    }

    public IISWebConfigWriter(Stream stream, XmlWriterSettings? xmlWriterSettings = null)
    {
        _writer = XmlWriter.Create(stream, xmlWriterSettings ?? DefaultSettings);
    }

    public IISWebConfigWriter(StringBuilder stringBuilder, XmlWriterSettings? xmlWriterSettings = null)
    {
        _writer = XmlWriter.Create(stringBuilder, xmlWriterSettings ?? DefaultSettings);
    }
    
    public async Task WriteWebConfig(BmSecurityHeadersConfig config, bool removeServerHeaders = true, bool enforceHttps = true)
    {
        await _writer.WriteStartDocumentAsync();
        await _writer.WriteStartElementAsync("configuration");
        
        if (removeServerHeaders)
        {
            await WriteSystemWeb();
        }

        await WriteSystemWebServer(config, removeServerHeaders, enforceHttps);
        
        await _writer.WriteEndElementAsync();
        await _writer.FlushAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _writer.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _writer.DisposeAsync();
    }

    private async Task WriteSystemWeb()
    {
        await _writer.WriteStartElementAsync("system.web");
        await _writer.WriteStartElementAsync("httpRuntime");
        await _writer.WriteAttributeAsync("enableVersionHeader", false);
        await _writer.WriteEndElementAsync();
        await _writer.WriteEndElementAsync();
    }

    private async Task WriteSystemWebServer(BmSecurityHeadersConfig config, bool removeServerHeaders, bool enforceHttps)
    {
        await _writer.WriteStartElementAsync("system.webServer");

        if (removeServerHeaders)
        {
            await WriteSecurity();
        }

        if (enforceHttps)
        {
            await WriteRewrite();
        }

        await WriteHttpProtocol(config, removeServerHeaders);
        
        await _writer.WriteEndElementAsync();
    }

    private async Task WriteSecurity()
    {
        await _writer.WriteStartElementAsync("security");
        await _writer.WriteStartElementAsync("requestFiltering");
        await _writer.WriteAttributeAsync("removeServerHeader",  true);
        await _writer.WriteEndElementAsync();
        await _writer.WriteEndElementAsync();
    }

    private async Task WriteRewrite()
    {
        await _writer.WriteStartElementAsync("rewrite");
        await _writer.WriteStartElementAsync("rules");
        await _writer.WriteStartElementAsync("rule");
        await _writer.WriteAttributeAsync("name",  "Enforce HTTPS");
        await _writer.WriteAttributeAsync("enabled",  true);

        await _writer.WriteStartElementAsync("match");
        await _writer.WriteAttributeAsync("url",  "(.*)");
        await _writer.WriteAttributeAsync("ignoreCase",  false);
        await _writer.WriteEndElementAsync();

        await _writer.WriteStartElementAsync("conditions");
        await _writer.WriteStartElementAsync("add");
        await _writer.WriteAttributeAsync("input",  "{HTTPS}");
        await _writer.WriteAttributeAsync("pattern",  "off");
        await _writer.WriteEndElementAsync();
        await _writer.WriteEndElementAsync();

        await _writer.WriteStartElementAsync("action");
        await _writer.WriteAttributeAsync("type",  "Redirect");
        await _writer.WriteAttributeAsync("url",  "https://{HTTP_HOST}/{R:1}");
        await _writer.WriteAttributeAsync("appendQueryString",  false);
        await _writer.WriteEndElementAsync();
        
        await _writer.WriteEndElementAsync();
        await _writer.WriteEndElementAsync();
        await _writer.WriteEndElementAsync();
    }

    private async Task WriteHttpProtocol(BmSecurityHeadersConfig config, bool removeServerHeaders)
    {
        await _writer.WriteStartElementAsync("httpProtocol");
        await _writer.WriteStartElementAsync("customHeaders");

        await WriteSecurityHeaders(config);

        if (removeServerHeaders)
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

    private async Task WriteSecurityHeaders(BmSecurityHeadersConfig config)
    {
        var fakeContext = new DefaultHttpContext();
        fakeContext.Request.IsHttps = true;
        var headersResult = new CustomHeadersResult();

        foreach (var policy in config.Values)
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