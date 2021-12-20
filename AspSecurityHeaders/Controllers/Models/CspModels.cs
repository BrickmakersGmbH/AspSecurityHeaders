using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders.Controllers.Models
{
    public class CspReportRequest
    {
        [Required]
        [JsonPropertyName("csp-report")]
        public CspReport CspReport { get; set; } = null!;
    }

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

        public IDictionary<string, string?> AsAttributes()
        {
            return GetType().GetProperties().ToDictionary(
                prop => prop.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? prop.Name,
                prop => prop.GetValue(this)?.ToString());
        }

        public override string ToString()
        {
            return $"CSP Violation in {DocumentUri}: Refused to load {BlockedUri} because of '{ViolatedDirective}' directive. " +
                   $"Details:\n{string.Join(Environment.NewLine, AsAttributes())}";
        }
    }
}