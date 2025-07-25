# BRICKMAKERS ASP.Net Security Headers

[![License](https://img.shields.io/github/license/BrickmakersGmbH/AspSecurityHeaders)](https://github.com/BrickmakersGmbH/AspSecurityHeaders/blob/main/LICENSE.txt)
[![CI-Pipeline](https://github.com/BrickmakersGmbH/AspSecurityHeaders/actions/workflows/ci.yml/badge.svg)](https://github.com/BrickmakersGmbH/AspSecurityHeaders/actions/workflows/ci.yml)
[![Brickmakers.AspSecurityHeaders Nuget Version](https://img.shields.io/nuget/v/Brickmakers.AspSecurityHeaders?label=Brickmakers.AspSecurityHeaders)](https://www.nuget.org/packages/Brickmakers.AspSecurityHeaders)
[![Brickmakers.AspSecurityHeaders.OrchardModule Nuget Version](https://img.shields.io/nuget/v/Brickmakers.AspSecurityHeaders.OrchardModule?label=Brickmakers.AspSecurityHeaders.OrchardModule)](https://www.nuget.org/packages/Brickmakers.AspSecurityHeaders.OrchardModule)
[![Brickmakers.AspSecurityHeaders.Generators Nuget Version](https://img.shields.io/nuget/v/Brickmakers.AspSecurityHeaders.Generators?label=Brickmakers.AspSecurityHeaders.Generators)](https://www.nuget.org/packages/Brickmakers.AspSecurityHeaders.Generators)

A small package for ASP.Net (Core) to automatically configure secure HTTP-Headers.

## Table of Contents

- [IMPORTANT CHANGES in version 2.1.0](#important-changes-in-version-210)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
    * [AspSecurityHeaders](#aspsecurityheaders)
        + [Using the Built-In CSP Report Controller](#using-the-built-in-csp-report-controller)
    * [Orchard Module](#orchard-module)
        + [Overwriting the Orchard CSP](#overwriting-the-orchard-csp)
        + [Support for Login with Microsoft/Azure AD](#support-for-login-with-microsoft-azure-ad)
    * [Generators](#generators)
        + [IIS web.config](#iis-webconfig)
- [Attributions and Background](#attributions-and-background)

<small><i><a href='https://ecotrust-canada.github.io/markdown-toc/'>Table of contents generated with
markdown-toc</a></i></small>

## IMPORTANT CHANGES in version 2.7.0

In 2.7.0, the `NetEscapades.AspNetCore.SecurityHeaders` have been updated to version 1.1.0, which includes some changes
to the actual header values being set. The changes are reflected in this package as well, so you might encounter some
deprecations and changes in the headers being set. The most notable changes are:

- The `Feature-Policy` header has been removed, as is no longer supported by browsers. The `Permissions-Policy`
  header is not affected by this change and will still be set.
- The default values for the `Permissions-Policy` header have been changed to be more restrictive. This means that
  some features that were previously allowed by default are now disabled by default. If you actively use these
  (unsecure) features, you will have to manually reenable them after the update. The newly disabled features are:
    - `fullscreen`
    - `picture-in-picture`
    - `sync-xhr`
- The package no longer differentiates between HTML and non-HTML responses and always sets the same headers for
  all responses. This means that the `Content-Security-Policy` header will always be set, even for API responses when 
  configured for normal websites. This includes the generated IIS `web.config` files.

## Features

- Secure defaults for HTTP-Headers, CSP, Cookies and more
- Opt-Out mechanism for different security controls
- Easily configurable via `IApplicationBuilder.UseBmSecurityHeaders()` extension
    - Or use `IApplicationBuilder.UseBmApiSecurityHeaders()` for API-Projects
- Developed and Maintained by the BRICKMAKERS Security Advisory Team
    - Based on the widely
      used [NetEscapades.AspNetCore.SecurityHeaders](https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders)
- Easy integration in any project and build pipelines
- Provides additional generator package to create config files with security headers for:
    - IIS `web.config` files

## Installation

This package is available on NuGet.org, you can simply add it to your C#-Project like any other dependency.

- Main Package: [Brickmakers.AspSecurityHeaders](https://www.nuget.org/packages/Brickmakers.AspSecurityHeaders/)
- Generators
  Package: [Brickmakers.AspSecurityHeaders.Generators](https://www.nuget.org/packages/Brickmakers.AspSecurityHeaders.Generators/)

## Usage

### AspSecurityHeaders

For the standard features of the Security Headers you only need to install `Brickmakers.AspSecurityHeaders`.

To get started, all you have to to is to register the middleware in the `Configure` method. This should happen **at the
beginning** of the method to ensure the headers are added to all responses, as different middlewares might end
processing early, which would prevent the headers from being set:

```cs
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

```cs
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

#### Using the Built-In CSP Report Controller

The library includes a ready-made API-Controller to automatically report CSP-Violations. It will provide an endpoint to
be used by the browser to report CSP errors and passes them to a customizable handler function. If you want to use the
controller, there are a few steps that need to be taken.

First, you have to add the controller to your controllers by extending the `CspReportControllerBase`:

```cs
[ApiController]
[Route("[controller]")]
public class CspReportController : CspReportControllerBase
{
    protected override Task HandleCspReport(CspReport cspReport)
    {
        // Implement logging or other handling here
        // IMPORTANT: If you log the report values, you should sanitized them to prevent log forgery attacks
        // See: https://owasp.org/www-community/attacks/Log_Injection
        
        return Task.CompletedTask;
    }
}
```

If you are using the standard `Microsoft.Extensions.Logging.ILogger` for logging, you can use a handy extension method
on the logger that automatically handles formatting and also logs the properties of the report in case you are using
a structured logging backend like App Insights.

```cs
[ApiController]
[Route("[controller]")]
public class CspReportController : CspReportControllerBase
{
    protected override Task HandleCspReport(CspReport cspReport)
    {
        _logger.LogCspReport(cspReport); // default log level ist "Error", but can be adjusted
        return Task.CompletedTask;
    }
}
```

Next, you have to add the controller to the MVC instance inside of the `ConfigureServices` method. Typically,
the `AddMvc` method is used, but you can also use any other of the MVC initializers, like for example `AddControllers`
in case of a pure API. In addition to registering controllers, you also need to add the CSP-Report content type. You can
simply use the `AddCspMediaType` method for that:

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc() // works on .AddRazorPages() and .AddControllers() as well
        .AddCspMediaType();
}
```

In the case that this is the first controller you add to your project, you also need to ensure that controllers are
correctly mapped to endpoints. You can do so via the `UseEndpoints` method at the end of `Configure`:

```cs
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
the `UseBmSecurityHeaders` by adding `AddReportUri` to the CSP. There you should set the path to the previously defined
CSP controller. In this example, the controller path was defined as `CspReport`.

```cs
public void Configure(IApplicationBuilder app)
{
    app.UseBmSecurityHeaders(collection => collection  // Or .UseBmApiSecurityHeaders for APIs
        .AddBmContentSecurityPolicy(builder =>
        {
            // setup your CSP
            // ...
            
            builder.AddReportUri().To("/CspReport");
        }));
    // ...
}
```

In case you also have additional projects that should also report to this controller, or in case you separate API and
web project, the controller will always be accessible via `https://<host>/CspReport`. You can use it as any other CSP
reporting endpoint.

### Orchard Module

If you are working with [Orchard Core](https://orchardcore.net/), then instead of using the Security Headers package
directly, you should instead use the `Brickmakers.AspSecurityHeaders.OrchardModule` package, which itself is an orchard
module that automatically configures the security headers for you. To use it, follow the standard Steps to add an
Orchard module as dependency:

1. Add the NuGet package reference
2. Update your `Manifest.cs` and add `Brickmakers.AspSecurityHeaders.OrchardModule` as dependency
3. Enable MVC in your application `Startup.cs`: `services.AddOrchardCore().AddMvc();`
4. For Orchard CMS installations: Enter the "Features" Admin Menu and manually enable the module

With the, the module is automatically loaded and activated. It will:

1. Enable all standard security headers, including a customized CSP
2. Register the CSP report controller under `/CspReport`

To customize the security headers, you can basically follow the standard instructions of the normal Security headers
package, with 2 exceptions: Use `UseOrchardBmSecurityHeaders` and `AddOrchardBmContentSecurityPolicy` instead of their 
"normal" counterparts:

```cs
public void Configure(IApplicationBuilder app)
{
    // ! Should be the first step in the Configure method

    // Only needed if customization is required
    app.UseOrchardBmSecurityHeaders(config => config
        .AddOrchardBmContentSecurityPolicy(/* ... */) // csp config
        // ... other configuration, just like with the normal security headers
    );
}
```

> **Note:** Orchard core is not the most security aware framework. The default CSP that is required to make it work
> includes `unsafe-inline` `unsafe-eval`. Be aware that for a security sensitive application, it should be carefully
> evaluated if orchard core is the right choice, or whether critical components should be provided in a pure ASP.net
> application that allows for tighter security controls and a better CSP.

#### Overwriting the Orchard CSP

When customizing the Orchard CSP, you can simply add new rules to the existing ones. This will not overwrite the
standard orchard rules anymore. If you need to disable the standard rules, you can use the optional `clear` parameter.
For example, if a script source should be added but the image sources should be cleared and replaced, it would look
like the following:

```cs
public void Configure(IApplicationBuilder app)
{
    app.UseOrchardBmSecurityHeaders(config => config
        .AddOrchardBmContentSecurityPolicy(builder => 
        {
            builder.AddScriptSrc() // Adds new diretives
                .From("https://example.com");
            builder.AddImgSrc(clear: true) // Replaces directives (usually not needed)
                .Self()
                .From("https://example.com");
        })
    );
}
```

#### Support for Login with Microsoft/Azure AD

If you want to allow a login with Microsoft in your orchard application, special cookie policy rules need to be added so
that azure can pass the authentication result back to the orchard application. Additionally, some CSP rules need to be
adjusted, as otherwise your page cannot redirect to microsoft. You can either manually configure the rules via the
`AddCookieOption` and the CSP builder, or use the helper methods that do that for you:

```cs
public void Configure(IApplicationBuilder app)
{
    app.UseOrchardBmSecurityHeaders(config => config
        .AddMicrosoftLoginCookieWhitelist()
        .AddOrchardBmContentSecurityPolicy(cspBuilder => {
            cspBuilder.AddFormAction()
                .MicrosoftLogin();
        })
    );
}
```

### Generators

To use the generators, you have to install the `Brickmakers.AspSecurityHeaders.Generators` package. The you can use the
various writers to generate your configuration.

#### IIS web.config

To generate a web.config file with security headers, you can use the `IISWebConfigWriter` class:

```cs
await IISWebConfigWriter.Create() // or .CreateApi()
    .SetBmSecurityHeadersConfig(config => config
        .AddBmContentSecurityPolicy(builder =>
        {
            builder.AddScriptSrc().Self();
            builder.AddStyleSrc().Self();
            builder.AddImgSrc().Self();
        }))
    .EnforceHttps(false)
    .Run("web.config");
```

With the `SetBmSecurityHeadersConfig`, you can configure your security headers in exactly the same way as with the
standard security headers package. In addition to that, there are also some extra configuration options that are only
available with web.config files. These are:

- XML Writer configuration for controlling how the generated XML is formatted
- Advanced removal of server identifying headers
- Enforce HTTPS
- Flags to control if the generated headers should be for TLS

## Attributions and Background

This project is heavily based
on [NetEscapades.AspNetCore.SecurityHeaders](https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders),
thanks to everyone involved on that project.

The reason this package exists is that it enforces even stricter defaults than the original package and adds additional
features. It has not been integrated into the original security headers, as some of these features would be breaking
changes and too strict for some users.

However, we at BRICKMAKERS prefer to use tight secure defaults, which is why we created this package. It will always set
everything to no by default and may add new, even more restricting headers in the future.
