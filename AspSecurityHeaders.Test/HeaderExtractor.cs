using System.Collections.Generic;
using System.Linq;

namespace Brickmakers.AspSecurityHeaders.Test;

public static class HeaderExtractor
{
    private static IEnumerable<string> ExtractPermissions(string permissions)
    {
        var parenthesisCount = 0;
        var begin = 0;
        for (var index = 0; index < permissions.Length; ++index)
        {
            switch (permissions[index])
            {
                case '(':
                    parenthesisCount++;
                    break;
                case ')':
                    parenthesisCount--;
                    break;
                case ',':
                    if (parenthesisCount == 0)
                    {
                        yield return permissions[begin..index].Trim();
                        begin = index + 1;
                    }

                    break;
            }
        }

        yield return permissions[begin..].Trim();
    }

    private static KeyValuePair<string, string> ParsePermission(string permission)
    {
        var equalsIndex = permission.IndexOf('=');
        var name = permission[..equalsIndex];
        var value = permission[(equalsIndex + 1)..];
        return new KeyValuePair<string, string>(name, value);
    }

    public static IReadOnlyDictionary<string, string> ParsePermissions(
        IEnumerable<string> permissions
    )
    {
        return new Dictionary<string, string>(
            permissions.SelectMany(ExtractPermissions).Select(ParsePermission)
        );
    }

    private static KeyValuePair<string, string> ParseFeature(string feature)
    {
        var spaceIndex = feature.IndexOf(' ');
        var name = feature[..spaceIndex];
        var value = feature[(spaceIndex + 1)..];
        return new KeyValuePair<string, string>(name, value);
    }

    public static IReadOnlyDictionary<string, string> ParseFeatures(IEnumerable<string> features)
    {
        return new Dictionary<string, string>(
            features
                .SelectMany(featureValues => featureValues.Split(';'))
                .Select(feature => feature.Trim())
                .Where(feature => feature.Length > 0)
                .Select(ParseFeature)
        );
    }

    private static KeyValuePair<string, IEnumerable<string>> ParseCsp(string csp)
    {
        var values = csp.Trim().Split(' ');
        return new KeyValuePair<string, IEnumerable<string>>(values[0], values[1..]);
    }

    public static IReadOnlyDictionary<string, IEnumerable<string>> ParseCsp(IEnumerable<string> csp)
    {
        return new Dictionary<string, IEnumerable<string>>(
            csp.SelectMany(cspValues => cspValues.Split(';'))
                .Select(cspEntry => cspEntry.Trim())
                .Where(cspEntry => cspEntry.Length > 0)
                .Select(ParseCsp)
        );
    }

    private static KeyValuePair<string, string> ParseCookieValue(string cookieValue)
    {
        var equalsIndex = cookieValue.IndexOf('=');
        if (equalsIndex == -1)
        {
            return new KeyValuePair<string, string>(cookieValue, "");
        }

        var name = cookieValue[..equalsIndex];
        var value = cookieValue[(equalsIndex + 1)..];
        return new KeyValuePair<string, string>(name, value);
    }

    public static IReadOnlyDictionary<string, string> ParseCookie(string cookie)
    {
        return new Dictionary<string, string>(
            cookie.Split(';').Select(value => value.Trim()).Select(ParseCookieValue)
        );
    }
}
