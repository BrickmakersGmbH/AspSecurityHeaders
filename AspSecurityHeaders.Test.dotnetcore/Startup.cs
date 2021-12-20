using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using de.brickmakers.SecurityEngineering.AspSecurityHeaders;
using de.brickmakers.SecurityEngineering.AspSecurityHeaders.BmCookiePolicyExtensions;
using de.brickmakers.SecurityEngineering.AspSecurityHeaders.Controllers;
using de.brickmakers.SecurityEngineering.AspSecurityHeaders.HeaderPolicyCollectionExtensions;
using Microsoft.AspNetCore.Http;

namespace AspSecurityHeaders.Test.dotnetcore
{
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
                .AddSecurityControllers(); // works on AddRazorPages and AddControllers as well
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseBmSecurityHeaders(collection => collection
                .AddBmContentSecurityPolicy(builder =>
                {
                    builder.AddScriptSrc()
                        .Self()
                        .ReportSample();
                    builder.AddStyleSrc()
                        .Self()
                        .ReportSample();
                    builder.AddImgSrc().Self();
                    builder.AddReportUri().To("https://localhost:5001/CspReport");
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
                context.Response.Cookies.Append("TestCookie", "value", new CookieOptions()
                {
                    Path = "/",
                    Expires = DateTimeOffset.UtcNow.AddHours(1),
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
}