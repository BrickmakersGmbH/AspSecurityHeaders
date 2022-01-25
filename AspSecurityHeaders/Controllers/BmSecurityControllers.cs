using System.Linq;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace Brickmakers.AspSecurityHeaders.Controllers;

public static class BmSecurityControllers
{
    public static IMvcBuilder AddSecurityControllers(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder
            .AddApplicationPart(typeof(BmSecurityControllers).Assembly)
            .AddMvcOptions(options =>
            {
                var jsonInputFormatter = options.InputFormatters
                    .OfType<SystemTextJsonInputFormatter>()
                    .Single();
                jsonInputFormatter.SupportedMediaTypes.Add("application/csp-report");
            });
    }
}