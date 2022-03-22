using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace Brickmakers.AspSecurityHeaders.Controllers;

/// <summary>
///     Extensions to <see cref="IMvcBuilder" /> to add security controllers
/// </summary>
public static class BmSecurityControllers
{
    /// <summary>
    ///     Registers the "application/csp-report" content type with the application. This is required for the
    ///     <see cref="CspReportControllerBase" />, as otherwise your application will not be able to process reports as sent
    ///     by the browser.
    /// </summary>
    /// <param name="mvcBuilder">The <see cref="IMvcBuilder" /> to add media type to.</param>
    /// <returns>The mvcBuilder that was passed as this.</returns>
    public static IMvcBuilder AddCspMediaType(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder
            .AddMvcOptions(options =>
            {
                var jsonInputFormatter = options.InputFormatters
                    .OfType<SystemTextJsonInputFormatter>()
                    .Single();
                jsonInputFormatter.SupportedMediaTypes.Add("application/csp-report");
            });
    }
}