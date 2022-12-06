using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOrchardCore()
    .AddMvc()
    // // Orchard Specific Pipeline
    // .ConfigureServices( services => {

    // })
    // .Configure( (app, routes, services) => {

    // })
    ;

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseOrchardCore();

app.Run();