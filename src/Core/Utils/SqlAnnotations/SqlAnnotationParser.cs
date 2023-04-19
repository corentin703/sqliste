using System.Reflection;
using System.Text.RegularExpressions;
using Sqliste.Core.Contracts;
using Sqliste.Core.SqlAnnotations.SqlAnnotations;

namespace Sqliste.Core.Utils.SqlAnnotations;

public static class SqlAnnotationParser
{
    private const string SqlAnnotationPattern = @"#(?<Annotation>\w+)\s*(\((?<Content>.*)\))?";
    private const string SequencedParametersPattern = @"\s*(?<Value>""(\\""|[^""])+""+|(true|false|(\d|\.)+))(\s|,)*";
    private const string NamedParametersPattern = @"\s*(?<Name>\w+)\s*=\s*(?<Value>""(\\""|[^""])+""+|(true|false|(\d|\.)+))(\s|,)*";

    private static readonly Dictionary<string, Type> _annotationsByName = new();

    public static List<ISqlAnnotation> ParseSqlString(string sqlContent)
    {
        EnsureAnnotationIndexInitialized();

        List<ISqlAnnotation> annotations = new();

        MatchCollection matches = Regex.Matches(sqlContent, SqlAnnotationPattern, RegexOptions.Multiline);

        if (matches.Count == 0)
            return annotations;

        foreach (Match match in matches)
        {
            if (!match.Success) 
                continue;

            string annotationName = match.Groups["Annotation"].Value.Trim();
            string content = match.Groups["Content"].Value.Trim();

            annotations.Add(ParseSingle(annotationName, content));
        }

        return annotations;
    }

    private static ISqlAnnotation ParseSingle(string annotationName, string content)
    {
        ISqlAnnotation? annotation = null;

        if (!_annotationsByName.ContainsKey(annotationName))
            throw new SqlAnnotationNotFoundException(annotationName);

        Type annotationType = _annotationsByName[annotationName];

        if (!Regex.IsMatch(content, NamedParametersPattern))
        {
            object[] sequencedParameters = ExtractSequencedParams(content).ToArray();
            annotation = Activator.CreateInstance(annotationType, sequencedParameters) as ISqlAnnotation;

            if (annotation == null)
                throw new SqlAnnotationInstantiationException(annotationName);

            return annotation;
        }

        Dictionary<string, object> namedParameters = ExtractNamedParams(content);

        annotation = Activator.CreateInstance(annotationType) as ISqlAnnotation;

        if (annotation == null)
            throw new SqlAnnotationInstantiationException(annotationName);

        foreach (var parameterPair in namedParameters)
        {
            PropertyInfo? property = annotationType.GetProperty(parameterPair.Key);
            if (property == null)
                throw new SqlAnnotationPropertyNotFoundException(parameterPair.Key);

            property.SetValue(annotation, parameterPair.Value);
        }

        return annotation;
    }

    private static Dictionary<string, object> ExtractNamedParams(string content)
    {
        Dictionary<string, object> namedParameters = new();

        MatchCollection matches = Regex.Matches(content, NamedParametersPattern);
        foreach (Match match in matches)
        {
            string name = match.Groups["Name"].Value.Trim();
            string value = match.Groups["Value"].Value.Trim();

            namedParameters.Add(name, ParseValue(value, name));
        }

        return namedParameters;
    }

    private static List<object> ExtractSequencedParams(string content)
    {
        List<object> sequencedParameters = new();

        MatchCollection matches = Regex.Matches(content, SequencedParametersPattern);
        foreach (Match match in matches)
        {
            string value = match.Groups["Value"].Value.Trim();
            sequencedParameters.Add(ParseValue(value));
        }

        return sequencedParameters;
    }


    private static object ParseValue(string value, string? propertyName = null)
    {
        string trimmed = value.Trim();

        if (trimmed.StartsWith("\""))
            return trimmed.Trim('"');

        if (trimmed == "true")
            return true;

        if (trimmed == "false")
            return false;

        try
        {
            if (trimmed.Contains("."))
                return float.Parse(trimmed);

            return int.Parse(trimmed);
        }
        catch
        {
            if (!string.IsNullOrEmpty(propertyName))
                throw new SqlAnnotationPropertyValueParseException(propertyName, trimmed);

            throw new SqlAnnotationPropertyValueParseException(trimmed);
        }
    }

    private static void EnsureAnnotationIndexInitialized()
    {
        if (_annotationsByName.Count > 0) 
            return;

        Type annotationInterface = typeof(ISqlAnnotation);
        List<Type> annotationTypes = annotationInterface.Assembly.GetExportedTypes()
            .Where(type => type.FullName != annotationInterface.FullName && type.IsAssignableTo(annotationInterface))
            .ToList();

        annotationTypes.ForEach(type =>
        {
            string name = type.Name.Replace("SqlAnnotation", "");
            _annotationsByName[name] = type;
        });
    }
}