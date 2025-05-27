using System.Text;
using System.Xml;
using Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;

namespace Brickmakers.AspSecurityHeaders.Generators;

/// <summary>
///     A tool to generate a IIS <c>web.config</c> file containing security headers and other, security-related features.
/// </summary>
// ReSharper disable once InconsistentNaming
public class IISWebConfigWriter
{
    private readonly IISWebConfigWriterSettings _settings = new();

    private IISWebConfigWriter() { }

    /// <summary>
    ///     Creates a new <see cref="IISWebConfigWriter" /> using the standard security headers.
    /// </summary>
    /// <returns>A new <see cref="IISWebConfigWriter" /> instance.</returns>
    /// <seealso cref="DefaultBmSecurityHeaders.AddDefaultBmSecurityHeaders">HeaderPolicyCollection.AddDefaultBmSecurityHeaders</seealso>
    public static IISWebConfigWriter Create()
    {
        var builder = new IISWebConfigWriter();
        builder._settings.BmSecurityHeadersConfig.AddDefaultBmSecurityHeaders();
        return builder;
    }

    /// <summary>
    ///     Creates a new <see cref="IISWebConfigWriter" /> using the REST-API security headers.
    /// </summary>
    /// <returns>A new <see cref="IISWebConfigWriter" /> instance.</returns>
    /// <seealso cref="DefaultBmSecurityHeaders.AddDefaultBmApiSecurityHeaders">HeaderPolicyCollection.AddDefaultBmApiSecurityHeaders</seealso>
    public static IISWebConfigWriter CreateApi()
    {
        var builder = new IISWebConfigWriter();
        builder._settings.BmSecurityHeadersConfig.AddDefaultBmApiSecurityHeaders();
        return builder;
    }

    /// <summary>
    ///     Replaces the settings for the XML writer.
    /// </summary>
    /// <param name="xmlWriterSettings">The new settings to be used.</param>
    /// <returns>A reference to this.</returns>
    /// <remarks>
    ///     The method will internally set the <see cref="XmlWriterSettings.Async" /> property to true, as it is required
    ///     for the writer to work.
    /// </remarks>
    public IISWebConfigWriter SetXmlWriterSettings(XmlWriterSettings xmlWriterSettings)
    {
        _settings.XmlWriterSettings = xmlWriterSettings;
        _settings.XmlWriterSettings.Async = true;
        return this;
    }

    /// <summary>
    ///     Configures the settings for the XML writer.
    /// </summary>
    /// <param name="configure">A callback to configure the <see cref="XmlWriterSettings" />.</param>
    /// <returns>A reference to this.</returns>
    /// <remarks>
    ///     The method will internally set the <see cref="XmlWriterSettings.Async" /> property to true, as it is required
    ///     for the writer to work.
    /// </remarks>
    public IISWebConfigWriter SetXmlWriterSettings(Action<XmlWriterSettings> configure)
    {
        configure.Invoke(_settings.XmlWriterSettings);
        _settings.XmlWriterSettings.Async = true;
        return this;
    }

    /// <summary>
    ///     Replaces the <see cref="BmSecurityHeadersConfig" /> to be used to add security headers to the web.config.
    /// </summary>
    /// <param name="bmSecurityHeadersConfig">The new config to be used.</param>
    /// <returns>A reference to this.</returns>
    public IISWebConfigWriter SetBmSecurityHeadersConfig(
        BmSecurityHeadersConfig bmSecurityHeadersConfig
    )
    {
        _settings.BmSecurityHeadersConfig = bmSecurityHeadersConfig;
        return this;
    }

    /// <summary>
    ///     Configures the <see cref="BmSecurityHeadersConfig" /> to be used to add security headers to the web.config.
    /// </summary>
    /// <param name="configure">A callback to configure the <see cref="BmSecurityHeadersConfig" />.</param>
    /// <returns>A reference to this.</returns>
    /// <remarks>
    ///     The configured object will already have the default security headers set, depending on the creation method
    ///     that was used.
    /// </remarks>
    public IISWebConfigWriter SetBmSecurityHeadersConfig(Action<BmSecurityHeadersConfig> configure)
    {
        configure.Invoke(_settings.BmSecurityHeadersConfig);
        return this;
    }

    /// <summary>
    ///     Configures if common server headers should be removed. If enabled (the default), the following headers will be
    ///     removed:
    ///     <ul>
    ///         <li>Server Version</li>
    ///         <li><c>Server</c>-Header</li>
    ///         <li><c>X-Powered-By</c>-Header</li>
    ///     </ul>
    /// </summary>
    /// <param name="removeServerHeaders">Specifies if server header removals should be added or not.</param>
    /// <returns>A reference to this.</returns>
    public IISWebConfigWriter RemoveServerHeaders(bool removeServerHeaders)
    {
        _settings.RemoveServerHeaders = removeServerHeaders;
        return this;
    }

    /// <summary>
    ///     Configures if a HTTPS redirect rule should be added to the generated config. When enabled (the default), a
    ///     automatic rewrite rule will enforce a redirect to HTTPS, whenever HTTP is used.
    /// </summary>
    /// <param name="enforceHttps">Specifies if the rewrite rule should be added or not.</param>
    /// <returns>A reference to this.</returns>
    public IISWebConfigWriter EnforceHttps(bool enforceHttps)
    {
        _settings.EnforceHttps = enforceHttps;
        return this;
    }

    /// <summary>
    ///     Configures whether HTTPS-Only security headers should be added to the generated config. By default, HTTPS headers
    ///     are added.
    /// </summary>
    /// <param name="writeHttpsHeaders">Specifies if the headers should be added or not.</param>
    /// <returns>A reference to this.</returns>
    public IISWebConfigWriter WriteHttpsHeaders(bool writeHttpsHeaders)
    {
        _settings.WriteHttpsHeaders = writeHttpsHeaders;
        return this;
    }

    /// <summary>
    ///     Configures whether HTTPS-Only security headers should be added to the generated config. By default, HTTPS headers
    ///     are added.
    /// </summary>
    /// <param name="writeTlsHeaders">Specifies if the headers should be added or not.</param>
    /// <returns>A reference to this.</returns>
    /// <remarks>This method has been renamed to <see cref="WriteHttpsHeaders" /></remarks>
    [Obsolete($"Method was renamed to {nameof(WriteHttpsHeaders)}")]
    public IISWebConfigWriter WriteTlsHeaders(bool writeTlsHeaders)
    {
        return WriteHttpsHeaders(writeTlsHeaders);
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Obsolete(
        "Method has no effect as the feature was dropped by AspSecurityHeaders and will be removed in a future version."
    )]
    public IISWebConfigWriter WriteHtmlHeaders(bool writeHtmlHeaders)
    {
        return this;
    }

    [Obsolete(
        "Method has no effect as the feature was dropped by AspSecurityHeaders and will be removed in a future version."
    )]
    public IISWebConfigWriter WriteHttpHeaders(bool writeHttpHeaders)
    {
        return this;
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    ///     Runs the writer to generate the web.config. The config gets written to the specified path.
    /// </summary>
    /// <param name="path">The path of the file to write the config to.</param>
    /// <seealso cref="XmlWriter.Create(string,System.Xml.XmlWriterSettings)" />
    public async Task Run(string path)
    {
        await using var writer = new IISWebConfigWriterImpl(
            XmlWriter.Create(path, _settings.XmlWriterSettings),
            _settings
        );
        await writer.Run();
    }

    /// <summary>
    ///     Runs the writer to generate the web.config. The config gets written to the specified stream.
    /// </summary>
    /// <param name="stream">The stream to write the config to.</param>
    /// <seealso cref="XmlWriter.Create(System.IO.Stream,System.Xml.XmlWriterSettings)" />
    public async Task Run(Stream stream)
    {
        await using var writer = new IISWebConfigWriterImpl(
            XmlWriter.Create(stream, _settings.XmlWriterSettings),
            _settings
        );
        await writer.Run();
    }

    /// <summary>
    ///     Runs the writer to generate the web.config. The config gets written to the specified text writer.
    /// </summary>
    /// <param name="textWriter">The text writer to write the config to.</param>
    /// <seealso cref="XmlWriter.Create(System.IO.TextWriter,System.Xml.XmlWriterSettings)" />
    public async Task Run(TextWriter textWriter)
    {
        await using var writer = new IISWebConfigWriterImpl(
            XmlWriter.Create(textWriter, _settings.XmlWriterSettings),
            _settings
        );
        await writer.Run();
    }

    /// <summary>
    ///     Runs the writer to generate the web.config. The config gets written to the specified string builder.
    /// </summary>
    /// <param name="stringBuilder">The text writer to write the config to.</param>
    /// <seealso cref="XmlWriter.Create(System.Text.StringBuilder,System.Xml.XmlWriterSettings)" />
    public async Task Run(StringBuilder stringBuilder)
    {
        await using var writer = new IISWebConfigWriterImpl(
            XmlWriter.Create(stringBuilder, _settings.XmlWriterSettings),
            _settings
        );
        await writer.Run();
    }
}
