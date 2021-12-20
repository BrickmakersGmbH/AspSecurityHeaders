using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace de.brickmakers.SecurityEngineering.AspSecurityHeaders.Controllers
{
    public static class BmSecurityControllers
    {
        public static IMvcBuilder AddSecurityControllers(this IMvcBuilder mvcBuilder)
        {
            return mvcBuilder
                .AddApplicationPart(typeof(BmSecurityControllers).Assembly)
                .AddMvcOptions(options =>
                {
                    var jsonInputFormatter = options.InputFormatters
                        .OfType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonInputFormatter>()
                        .Single();
                    jsonInputFormatter.SupportedMediaTypes.Add("application/csp-report");
                });
        }
    }
}