using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Brickmakers.AspSecurityHeaders.Controllers.Models;

/// <summary>
///     A Request sent to the <see cref="CspReportController" /> containing a CSP report.
/// </summary>
public class CspReportRequest
{
    /// <summary>
    ///     The <see cref="CspReport" /> of the request
    /// </summary>
    [Required]
    [JsonPropertyName("csp-report")]
    public CspReport CspReport { get; set; } = null!;
}

/// <summary>
///     A CSP-Report as it is sent by the browsers. This model maps the JSON-Fields of such a request. See
///     <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy">Content-Security-Policy</a>
/// </summary>
public class CspReport
{
    [JsonPropertyName("document-uri")]
    public string? DocumentUri { get; set; }

    [JsonPropertyName("referrer")]
    public string? Referrer { get; set; }

    [JsonPropertyName("violated-directive")]
    public string? ViolatedDirective { get; set; }

    [JsonPropertyName("effective-directive")]
    public string? EffectiveDirective { get; set; }

    [JsonPropertyName("original-policy")]
    public string? OriginalPolicy { get; set; }

    [JsonPropertyName("blocked-uri")]
    public string? BlockedUri { get; set; }

    [JsonPropertyName("status-code")]
    public int? StatusCode { get; set; }

    [JsonPropertyName("disposition")]
    public string? Disposition { get; set; }

    [JsonPropertyName("script-sample")]
    public string? ScriptSample { get; set; }

    /// <summary>
    ///     Generates a dictionary containing all the fields of the CSP report.
    /// </summary>
    /// <returns>A dictionary with all fields of the CSP.</returns>
    public IDictionary<string, string?> AsAttributes()
    {
        return GetType().GetProperties().ToDictionary(
            prop => prop.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? prop.Name,
            prop => prop.GetValue(this)?.ToString());
    }

    /// <inheritdoc cref="object.ToString()" />
    public override string ToString()
    {
        return
            $"CSP Violation in {DocumentUri}: Refused to load {BlockedUri} because of '{ViolatedDirective}' directive. " +
            $"Details:\n{string.Join(Environment.NewLine, AsAttributes())}";
    }
}