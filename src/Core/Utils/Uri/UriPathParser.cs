using System.Text.RegularExpressions;

namespace Sqliste.Core.Utils.Uri;

public static class UriPathParser
{
    private const string UriPathPattern = @"^(?<path>[^\?]+)?.*$";

    public static string ExtractUriPath(string fullUri)
    {
        Match uriMatch = Regex.Match(fullUri, UriPathPattern);

        if (!uriMatch.Success)
            throw new ArgumentException(nameof(fullUri), "fullUri isn't an URI");

        return uriMatch.Groups["path"].Value;
    }
}