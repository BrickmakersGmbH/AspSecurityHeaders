# BRICKMAKERS ASP.Net Security Headers

[![License](https://img.shields.io/github/license/BrickmakersGmbH/AspSecurityHeaders)](https://github.com/BrickmakersGmbH/AspSecurityHeaders/blob/main/LICENSE.txt)
[![CI-Pipeline](https://github.com/BrickmakersGmbH/AspSecurityHeaders/actions/workflows/ci.yml/badge.svg)](https://github.com/BrickmakersGmbH/AspSecurityHeaders/actions/workflows/ci.yml)
[![Nuget Version](https://img.shields.io/nuget/v/Brickmakers.AspSecurityHeaders)](https://www.nuget.org/packages/Brickmakers.AspSecurityHeaders)
[![Nuget Downloads](https://img.shields.io/nuget/dt/Brickmakers.AspSecurityHeaders)](https://www.nuget.org/packages/Brickmakers.AspSecurityHeaders)

A small package for ASP.Net (Core) to automatically configure secure HTTP-Headers.

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
    * [Using the Built-In CSP Report Controller](#using-the-built-in-csp-report-controller)
- [Attributions & Background](#attributions---background)

<small><i><a href='http://ecotrust-canada.github.io/markdown-toc/'>Table of contents generated with
markdown-toc</a></i></small>

## Features

- Secure defaults for HTTP-Headers, CSP, Cookies and more
- Opt-Out mechanism for different security controls
- Easily configurable via `IApplicationBuilder.UseBmSecurityHeaders()` extension
    - Or use `IApplicationBuilder.UseBmApiSecurityHeaders()` for API-Projects
- Developed and Maintained by the BRICKMAKERS Security Advisory Team
    - Based on the widely
      used [NetEscapades.AspNetCore.SecurityHeaders](https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders)
- Easy integration in any project and build pipelines

## Installation

This package is available on [NuGet.org](https://www.nuget.org/packages/Brickmakers.AspSecurityHeaders/), you can simply
add it to your C#-Project like any other dependency.

## Usage

To get started, all you have to to is to register the middleware in the `Configure` method. This should happen **at the
beginning** of the method to ensure the headers are added to all responses, as different middlewares might end
processing early, which would prevent the headers from being set:

```.cs
public void Configure(IApplicationBuilder app)
{
    // ! Should be the first step in the Configure method

    // For "normal" Websites or combinations of Websites and APIs
    app.UseBmSecurityHeaders();

    // For pure APIs
    app.UseBmApiSecurityHeaders();

    // continue as usual with configuring the application
    // ...
}
```

This will add *all* security headers, as well as a strict CSP and cookie policy. To further configure it and opt out of
certain security controls, you can use the `configure` parameter of the method. In the following example, scripts,
styles and images are allowed to be loaded from the current origin and the minimum cookie same site requirements are
reduced to be lax instead of strict.

```.cs
public void Configure(IApplicationBuilder app)
{
    app.UseBmSecurityHeaders(collection => collection  // Or .UseBmApiSecurityHeaders for APIs
        .AddBmContentSecurityPolicy(builder =>
        {
            builder.AddScriptSrc().Self();
            builder.AddStyleSrc().Self();
            builder.AddImgSrc().Self();
        })
        .SetMinimumSameSitePolicy(SameSiteMode.Lax));

    // ...
}
```

### Using the Built-In CSP Report Controller

The library includes a ready-made API-Controller to automatically report CSP-Violations. It will provide an endpoint to
be used by the browser to report CSP errors and log them as error message. If you want to use the controller, there are
a few steps that need to be taken.

First, you have to add the controller to the MVC instance inside of the `ConfigureServices` method. Typically,
the `AddMvc` method is used, but you can also use any other of the MVC initializers, like for example `AddControllers`
in case of a pure API.

```.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
        .AddSecurityControllers();
        // works on .AddRazorPages() and .AddControllers() as well
}
```

In the case that this is the first controller you add to your project, you also need to ensure that controllers are
correctly mapped to endpoints. You can do so via the `UseEndpoints` method at the end of `Configure`:

```.cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // do your normal setup
    // ...

    // at the end, UseEndpoints should already exist
    app.UseEndpoints(endpoints =>
    {
        // this one must be present
        endpoints.MapControllers();
        
        // other mappings, e.g. MapRazorPages, depends on your application
        // ...
    });
}
```

Finally, you need to actually set the report URI in the CSP. This can be done by adding it inside the CSP builder of
the `UseBmSecurityHeaders` by adding `AddBmReportController` to the CSP. This automatically sets the report uri to the
CSP controller on this server.

```.cs
public void Configure(IApplicationBuilder app)
{
    app.UseBmSecurityHeaders(collection => collection  // Or .UseBmApiSecurityHeaders for APIs
        .AddBmContentSecurityPolicy(builder =>
        {
            // setup your CSP
            // ...
            
            builder.AddBmReportController();
        })
        .SetMinimumSameSitePolicy(SameSiteMode.Lax));

    // ...
}
```

In case you also have additional projects that should also report to this controller, or in case you separate API and
web project, the controller will always be accessible via `https://<host>/CspReport`. You can use it as any other CSP
reporting endpoint.

## Attributions & Background

This project is heavily based
on [NetEscapades.AspNetCore.SecurityHeaders](https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders),
thanks to everyone involved on that project.

The reason this package exists is because it enforces even stricter defaults than the original package and adds
additional features. It has not been integrated into the original security headers, as some of these feature would be
breaking changes and too strict for some users.

However, we at BRICKMAKERS prefer to use tight secure defaults, which is why we created this package. It will always set
everything to no by default and may add new, even more restricting headers in the future.
