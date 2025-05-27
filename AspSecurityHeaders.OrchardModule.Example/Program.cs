using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Brickmakers.AspSecurityHeaders.OrchardModule.Example;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel(options => options.AddServerHeader = false);
                webBuilder.UseStartup<Startup>();
            });
    }
}
