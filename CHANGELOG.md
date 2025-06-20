# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres
to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 2.7.0 - 2025-05-26

### Changed
- Updated dependencies
- Updated `NetEscapades.AspNetCore.SecurityHeaders` to 1.1.0
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

## 2.6.1 - 2024-12-10

### Changed

- Updated dependencies
    - Updated Orchard Core to 2.1

## 2.6.0 - 2024-11-21

### Added

- Added explicit support for .Net 9.0

### Removed

- Removed support for .Net 6.0

### Changed

- Updated dependencies
    - Updated Orchard Core to 2.0

## 2.5.2 - 2024-08-29

### Added

- Added explicit support for .Net 8.0

### Removed

- Removed support for .Net 7.0

### Changed

- Updated dependencies
    - Updated Security Headers to 0.23.0

### Fixed

- Fixed OrchardCore Module
    - Only supports .Net 8.0, as OrchardCore 1.8 dropped support for older versions

## 2.5.1 - 2023-09-12

### Fixed

- Fixed a bug the prevented the `BmFeaturePolicy` from being configured correctly, when the feature policy generation
  is enabled.

## 2.5.0 - 2023-09-12

### Changed

- Updated Orchard Core Module to use Orchard Core Version 1.7.0

## 2.4.0 - 2023-06-26

### Changed

- Deprecated `CspReport.AsAttributes` and replaced with `.ToDictionary`
- Added `ILogger.LogCspReport` extension for easy logging of a `CspReport` when using `ILogger`
- Improved logging format of csp reports in the Orchard module

## 2.3.0 - 2023-06-13

### Added

- Explicit support for .Net 7.0
- `UnconfigureCacheControl` method to allow to exclude cache control from being configured with the security headers

### Changed

- Replaced default CSP builder with custom wrapper that allows for directives to be updated
- Simplified CSP for orchard module

### Deprecated

### Removed

- Support for .Net Core 3.1

### Fixed

- Updated dependencies

### Security

- Escape csp report values for logger in orchard module endpoint

## 2.2.1 - 2022-12-15

### AspSecurityHeaders.OrchardModule

- Improved handling of microsoft logins
    - Renamed `AddAzureLoginCookieWhitelist` to `AddMicrosoftLoginCookieWhitelist`
    - Added CSP form action builder extension

## 2.2.0 - 2022-12-08

### Added

- New orchard core MVC module that enables the security headers for any orchard project
    - Includes adjusted CSP that works out of the box for a standard orchard project
    - Automatically adds the CSP report controller
    - Customized `UseOrchardBmSecurityHeaders` method to configure the headers for orchard projects
    - Includes `AddAzureLoginCookieWhitelist` that automatically configures the cookie policy for an Azure AD login

## 2.1.0 - 2022-12-05

### Added

- Enabled strict site isolation by enabling the COEP, COOP and CORP headers
    - Read https://web.dev/why-coop-coep/ for mor details, on why they are needed and what they do
    - Should you encounter problems, you can use overwrite their usage to only report errors via
      the `AddCrossOriginXXXPolicy` methods
- The `X-Powered-By` header now gets automatically removed as well

## 2.0.0 - 2022-03-22

This release contains breaking changes. See README for more details.

### Added

- New `CspReportControllerBase` controller base class that can be extended
    - Replaces the old, built-in CSP controller, as the automatic registration was problematic
    - Example on how to use it
- `AddCspMediaType`-Method to add the CSP media type to an `IMvcBuilder`

### Removed

- Built-In CSP-Controller, as the automatic registration was problematic
    - You can use the `CspReportControllerBase` instead, see README

## 1.3.1 - 2022-02-03

### Added

- Added XML-Documentation of all public members
- Added symbols packages

## 1.3.0 - 2022-02-02

### Added

- Created the new `Brickmakers.AspSecurityHeaders.Generators` package
    - Can generate an IIS `web.config` from the security headers config

## 1.2.2 - 2022-01-26

### Added

- Added package icon

## 1.2.1 - 2022-01-26

### Changed

- First public release on GitHub and NuGet.org

## 1.2.0 - 2021-12-21

### Added

- `CspReportController`: Easily report CSP violations via the built-in controller
- Integration Tests
- Support for .Net 6

## 1.1.0 - 2021-10-21

### Added

- `UseBmApiSecurityHeaders`: Add Configuration method for pure APIs

## 1.0.2 - 2021-10-12

### Security

- Disable HSTS preload by default

## 1.0.1 - 2021-10-11

### Added

- Initial Release
