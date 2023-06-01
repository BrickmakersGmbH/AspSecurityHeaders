using System;
using Brickmakers.AspSecurityHeaders.BmCookiePolicyExtensions;
using Brickmakers.AspSecurityHeaders.Controllers;
using Brickmakers.AspSecurityHeaders.HeaderPolicyCollectionExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brickmakers.AspSecurityHeaders.Example;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc()
            .AddCspMediaType(); // works on AddRazorPages and AddControllers as well
        services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // create "fake" server header to be removed by security headers
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("Server", "ASP.Net Core");
            await next();
        });

        app.UseBmSecurityHeaders(collection => collection
            .AddBmContentSecurityPolicy(builder =>
            {
                builder.AddScriptSrc().Self();
                builder.AddStyleSrc().Self();
                builder.AddImgSrc().Self();
                builder.AddReportUri().To("/CspReport");
            })
            .SetMinimumSameSitePolicy(SameSiteMode.Lax));

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseSwagger();

        app.UseRouting();

        app.UseAuthorization();

        app.Use(async (context, next) =>
        {
            context.Response.Cookies.Append("TestCookie", "value", new CookieOptions
            {
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            await next();
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
        });
    }
}