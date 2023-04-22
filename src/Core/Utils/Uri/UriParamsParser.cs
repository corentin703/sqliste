using System.Text.RegularExpressions;

namespace Sqliste.Core.Utils.Uri;

public static class UriParamsParser
{
    private const string TemplateParamPattern = @"{(?<name>\w+\??)}";

    private record ParamsMetadata(string Template, string Name, bool IsRequired)
    {
        public string Template { get; set; } = Template;
        public string Name { get; set; } = Name;
        public bool IsRequired { get; set; } = IsRequired;
    }

    public static Dictionary<string, string> ParseUrlParams(string template, string uri)
    {
        Dictionary<string, string> urlParams = new();

        List<ParamsMetadata> paramsMetadata = GetParamMetadata(template);
        if (paramsMetadata.Count == 0)
            return urlParams;

        string pattern = GetPatternFromTemplate(template, paramsMetadata);
        Match paramsMatch = Regex.Match(uri, pattern);
        if (!paramsMatch.Success)
            return urlParams;

        foreach (ParamsMetadata paramMetadata in paramsMetadata)
        {
            Group group = paramsMatch.Groups[paramMetadata.Name];
            if (string.IsNullOrEmpty(group.Value))
                continue;
            
            urlParams.Add(group.Name, group.Value);
        }

        return urlParams;
    }

    private static List<ParamsMetadata> GetParamMetadata(string template)
    {
        List<ParamsMetadata> paramMetadata = new();

        foreach (Match match in Regex.Matches(template, TemplateParamPattern))
        {
            if (!match.Success)
                continue;

            string paramTemplate = match.Groups["name"].Value;
            bool isRequired = !paramTemplate.Contains("?");
            string paramName = paramTemplate.Replace("?", "");

            paramMetadata.Add(new ParamsMetadata(paramTemplate, paramName, isRequired));
        }

        return paramMetadata;
    }

    private static string GetPatternFromTemplate(string template, List<ParamsMetadata> paramsMetadata)
    {
        string temp = template.Replace("/", "\\/");

        foreach (ParamsMetadata paramMetadata in paramsMetadata)
        {
            string valueMathTemplate = paramMetadata.IsRequired
                ? $"\\/(?<{paramMetadata.Name}>[^\\/]+)"
                : $"\\/?(?<{paramMetadata.Name}>[^\\/]+)?";

            temp = temp.Replace($"\\/{{{paramMetadata.Template}}}", valueMathTemplate);
        }

        return temp;
    }
}