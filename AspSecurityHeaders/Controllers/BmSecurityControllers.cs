using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace Brickmakers.AspSecurityHeaders.Controllers;

/// <summary>
///     Extensions to <see cref="IMvcBuilder" /> to add security controllers
/// </summary>
public static class BmSecurityControllers
{
    /// <summary>
    ///     Adds all currently available security controllers to the application. Currently, those are:
    ///     <ul>
    ///         <li>
    ///             <see cref="CspReportController" />
    ///         </li>
    ///     </ul>
    /// </summary>
    /// <param name="mvcBuilder">The <see cref="IMvcBuilder" /> to add the controllers to.</param>
    /// <returns>The mvcBuilder that was passed as this.</returns>
    /// <remarks>
    ///     This only adds the controllers to the MVC context. In order for the to actually be available,
    ///     <see cref="ControllerEndpointRouteBuilderExtensions.MapControllers">IEndpointRouteBuilder.MapControllers()</see>
    ///     must also be called in your StartUp (See README for more details).
    /// </remarks>
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