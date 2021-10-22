# BRICKMAKRES ASP.Net Core Security Headers
A small .net core package for ASP.Net Core to automatically configure secure HTTP-Headers.

## Features
- Secure defaults for HTTP-Headers, CSP, Cookies and more
- Opt-Out mechanism for different security controls
- Easily configurable via `IApplicationBuilder.UseBmSecurityHeaders()` extension
- Developed and Maintained by the BRICKMAKERS Security Advisory

## Installation
First, you need to add the package source to your project. This is described
here: https://brickmakers.visualstudio.com/SecurityEngineering/_packaging?_a=connect&feed=security-engineering-dotnet-package-feed
Alternatively, you can check the guide below on how to add it via Rider.

However, since this package feed is configured to only provide this package, you have to remove the
`<clear />` from the file. The `nuget.config` should now look like this:

```.xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="security-engineering-dotnet-package-feed" value="https://brickmakers.pkgs.visualstudio.com/SecurityEngineering/_packaging/security-engineering-dotnet-package-feed/nuget/v3/index.json" />
  </packageSources>
</configuration>
```

To add the package to your project, simply run the following command. The first time you do this,
you will be asked to log into the Azure DevOps portal

```.sh
dotnet add package --interactive de.brickmakers.SecurityEngineering.AspSecurityHeaders --version <version>
```

### Setup via Rider
First, select the `NuGet` Tab at the bottom of the screen. Next, select `Sources` and click the `+`
button to add a new package source.

![Select Nuget](doc/rider_1.png)

In the dialog, you have to enter a package name and the URL to the package feed. You can enter
anything you want for the package name, the URL should however be `https://brickmakers.pkgs.visualstudio.com/SecurityEngineering/_packaging/security-engineering-dotnet-package-feed/nuget/v3/index.json`.
Finally, press OK and Rider will ask you to log into the Azure Dev-Ops portal.

![Add Package Source](doc/rider_2.png)

Now you can add `de.brickmakers.SecurityEngineering.AspSecurityHeaders` just like any other normal
dependency.

## Usage
To get started, all you have to to is to register the middlware in the `Configure` method. This
should happen near the beginning of the method to ensure the headers are added to all responses, as
different middlewares might end processing early:

```.cs
public void Configure(IApplicationBuilder app)
{
    // For "normal" Websites
    app.UseBmSecurityHeaders();

    // For APIs
    app.UseBmApiSecurityHeaders();

    // ...
}
```

This will add *all* security headers, as well as a strict CSP and cookie policy. To further
configure it and opt out of certain security contols, you can use the `configure` parameter of the
method. In the following example, scripts, styles and images are allowed to be loaded from the
current origin and reduces the minimum cookie same site requirements to be lax instead of strict.

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

## Getting Help
If you have any problems with the package or it's installation, feel free to contact me (Felix Barz)
or any other member of the Security Advisory Team. We meet every second Wednesday.
