using System.Text.RegularExpressions;

namespace Sqliste.Core.Utils.Uri;

public static class UriQueryParamsParser
{
    private const string QueryParamPattern = @"(?<name>\w+)=(?<value>\w+)";

    public static Dictionary<string, string> ParseQueryParams(string queryString)
    {
        Dictionary<string, string> queryParams = new();

        foreach (Match queryParamMatch in Regex.Matches(queryString, QueryParamPattern))
        {
            if (!queryParamMatch.Success)
                continue;

            string name = queryParamMatch.Groups["name"].Value;
            string value = queryParamMatch.Groups["value"].Value;

            queryParams.TryAdd(name, value);
        }

        return queryParams;
    }
}