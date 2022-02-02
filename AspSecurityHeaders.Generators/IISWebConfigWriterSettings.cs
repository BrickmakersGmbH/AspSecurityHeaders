using System.Xml;

namespace Brickmakers.AspSecurityHeaders.Generators;

// ReSharper disable once InconsistentNaming
internal class IISWebConfigWriterSettings
{
    internal XmlWriterSettings XmlWriterSettings { get; set; } = new()
    {
        Indent = true,
        NewLineHandling = NewLineHandling.Entitize,
        CloseOutput = true,
        Async = true
    };

    internal BmSecurityHeadersConfig BmSecurityHeadersConfig { get; set; } = new();

    internal bool RemoveServerHeaders { get; set; } = true;

    internal bool EnforceHttps { get; set; } = true;

    internal bool WriteTlsHeaders { get; set; } = true;

    internal bool WriteHttpHeaders { get; set; } = true;
}