using System.Text;
using System.Xml;
using Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;

namespace Brickmakers.AspSecurityHeaders.Generators;

// ReSharper disable once InconsistentNaming
public class IISWebConfigWriter
{
    private readonly IISWebConfigWriterSettings _settings = new();

    private IISWebConfigWriter()
    {
    }

    public static IISWebConfigWriter Create()
    {
        var builder = new IISWebConfigWriter();
        builder._settings.BmSecurityHeadersConfig.AddDefaultBmSecurityHeaders();
        return builder;
    }

    public static IISWebConfigWriter CreateApi()
    {
        var builder = new IISWebConfigWriter();
        builder._settings.BmSecurityHeadersConfig.AddDefaultBmApiSecurityHeaders();
        return builder;
    }

    public IISWebConfigWriter SetXmlWriterSettings(XmlWriterSettings xmlWriterSettings)
    {
        _settings.XmlWriterSettings = xmlWriterSettings;
        return this;
    }

    public IISWebConfigWriter SetXmlWriterSettings(Action<XmlWriterSettings> configure)
    {
        configure.Invoke(_settings.XmlWriterSettings);
        return this;
    }

    public IISWebConfigWriter SetBmSecurityHeadersConfig(BmSecurityHeadersConfig bmSecurityHeadersConfig)
    {
        _settings.BmSecurityHeadersConfig = bmSecurityHeadersConfig;
        return this;
    }

    public IISWebConfigWriter SetBmSecurityHeadersConfig(Action<BmSecurityHeadersConfig> configure)
    {
        configure.Invoke(_settings.BmSecurityHeadersConfig);
        return this;
    }

    public IISWebConfigWriter RemoveServerHeaders(bool removeServerHeaders)
    {
        _settings.RemoveServerHeaders = removeServerHeaders;
        return this;
    }

    public IISWebConfigWriter EnforceHttps(bool enforceHttps)
    {
        _settings.EnforceHttps = enforceHttps;
        return this;
    }

    public IISWebConfigWriter WriteTlsHeaders(bool writeTlsHeaders)
    {
        _settings.WriteTlsHeaders = writeTlsHeaders;
        return this;
    }

    public IISWebConfigWriter WriteHttpHeaders(bool writeHttpHeaders)
    {
        _settings.WriteHttpHeaders = writeHttpHeaders;
        return this;
    }

    public async Task Run(string path)
    {
        await using var writer = new IISWebConfigWriterImpl(XmlWriter.Create(path, _settings.XmlWriterSettings), _settings);
        await writer.Run();
    }

    public async Task Run(Stream stream)
    {
        await using var writer = new IISWebConfigWriterImpl(XmlWriter.Create(stream, _settings.XmlWriterSettings), _settings);
        await writer.Run();
    }

    public async Task Run(TextWriter textWriter)
    {
        await using var writer = new IISWebConfigWriterImpl(XmlWriter.Create(textWriter, _settings.XmlWriterSettings), _settings);
        await writer.Run();
    }

    public async Task Run(StringBuilder stringBuilder)
    {
        await using var writer = new IISWebConfigWriterImpl(XmlWriter.Create(stringBuilder, _settings.XmlWriterSettings), _settings);
        await writer.Run();
    }
}