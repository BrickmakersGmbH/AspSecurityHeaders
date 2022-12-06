using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Brickmakers.AspSecurityHeaders.OrchardModule;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.PostConfigure<MvcOptions>(options =>
        {
            options.InputFormatters
                .OfType<InputFormatter>()
                .ToList()
                .ForEach(formatter => formatter.SupportedMediaTypes.Add("application/csp-report"));
        });
    }

    public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes,
        IServiceProvider serviceProvider)
    {
        builder.UseOrchardBmSecurityHeaders();
    }
}