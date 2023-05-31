# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres
to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased

### Added

- Explicit Support for .Net 7.0
- `UnconfigureCacheControl` method to allow to exclude cache control from being configured with the security headers.

### Changed

### Deprecated

### Removed

- Support for .Net Core 3.1

### Fixed

- Updated dependencies

### Security

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
