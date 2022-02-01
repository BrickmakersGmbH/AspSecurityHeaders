using Brickmakers.AspSecurityHeaders;
using Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;
using Microsoft.AspNetCore.Builder;

namespace AspSecurityHeaders.Tool;

internal static class Program
{
    private static async Task Main()
    {
        var config = new BmSecurityHeadersConfig();
        config.AddDefaultBmSecurityHeaders();
        config.AddCrossOriginEmbedderPolicy(builder => builder.RequireCorp());
        config.AddCrossOriginOpenerPolicy(builder => builder.SameOrigin());

        await using var writer = new IISWebConfigWriter("web.config");
        await writer.WriteWebConfig(config);
    }
}