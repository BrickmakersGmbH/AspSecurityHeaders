using System.Xml;

namespace Brickmakers.AspSecurityHeaders.Generators;

internal static class XmlWriterExtensions
{
    internal static Task WriteStartElementAsync(this XmlWriter writer, string name)
    {
        return writer.WriteStartElementAsync(null, name, null);
    }

    internal static Task WriteAttributeAsync(this XmlWriter writer, string name, string value)
    {
        return writer.WriteAttributeStringAsync(null, name, null, value);
    }

    internal static Task WriteAttributeAsync(this XmlWriter writer, string name, bool value)
    {
        return writer.WriteAttributeStringAsync(null, name, null, value.ToString().ToLower());
    }
}
