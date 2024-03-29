using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NetEscapades.AspNetCore.SecurityHeaders.Headers;
using NetEscapades.AspNetCore.SecurityHeaders.Infrastructure;

namespace Brickmakers.AspSecurityHeaders.HeaderPolicies;

internal class BmFeaturePolicyHeader : IHeaderPolicy
{
    private readonly PermissionsPolicyHeader _permissionsPolicy;

    public BmFeaturePolicyHeader(PermissionsPolicyHeader permissionsPolicy)
    {
        _permissionsPolicy = permissionsPolicy;
    }

    public string Header { get; } = new FeaturePolicyHeader("").Header;

    public void Apply(HttpContext context, CustomHeadersResult result)
    {
        var permissionsHeader = BuildPermissionsHeader(context);
        if (permissionsHeader == null)
        {
            return;
        }

        var featureHeader = permissionsHeader
            .Split(',')
            .Select(permission => permission.Trim())
            .Select(ConvertToFeature);

        result.SetHeaders[Header] = string.Join("", featureHeader);
    }

    private string? BuildPermissionsHeader(HttpContext context)
    {
        var permissionsResult = new CustomHeadersResult();
        _permissionsPolicy.Apply(context, permissionsResult, new HeaderPolicyCollection());
        if (!permissionsResult.SetHeaders.ContainsKey(_permissionsPolicy.Header))
        {
            return null;
        }

        var permissionsHeader = permissionsResult.SetHeaders[_permissionsPolicy.Header];
        return permissionsHeader;
    }

    private static string ConvertToFeature(string permission)
    {
        var equalsIndex = GetEqualsIndex(permission);
        var permissionName = permission.Substring(0, equalsIndex);
        var permissionValue = permission.Substring(equalsIndex + 1);
        var featureValues = ExtractPermissionValues(permissionValue)
            .Select(ConvertToFeatureDirective)
            .ToList();
        return $"{permissionName} {string.Join(" ", featureValues)};";
    }

    private static int GetEqualsIndex(string permission)
    {
        var equalsIndex = permission.IndexOf('=');
        if (equalsIndex < 0)
        {
            throw new ArgumentException("Invalid permission policy", permission);
        }

        return equalsIndex;
    }

    private static IEnumerable<string> ExtractPermissionValues(string permissionValue)
    {
        if (permissionValue.Equals("()"))
        {
            return new[] {"none"};
        }

        if (permissionValue.StartsWith("(") && permissionValue.EndsWith(")"))
        {
            return permissionValue.Substring(1, permissionValue.Length - 2).Split(' ');
        }

        return new[] {permissionValue};
    }

    private static string ConvertToFeatureDirective(string directive)
    {
        if (directive == "*")
        {
            return directive;
        }

        if (directive.StartsWith("\"") && directive.EndsWith("\""))
        {
            return directive.Substring(1, directive.Length - 2);
        }

        return $"'{directive}'";
    }
}