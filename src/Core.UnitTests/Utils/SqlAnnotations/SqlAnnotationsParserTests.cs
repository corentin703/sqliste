using Sqliste.Core.Contracts;
using Sqliste.Core.SqlAnnotations;
using Sqliste.Core.Utils.SqlAnnotations;

namespace Core.UnitTests.Utils.SqlAnnotations;

public class SqlAnnotationsParserTests
{
    [Theory]
    [InlineData("#Route")]
    [InlineData("#Route(\"/test\")")]
    //[InlineData("#Route( \"/te\\\"st\", \"ss\")")]
    public void TestInstantiation(string input)
    {
        List<ISqlAnnotation> annotations = SqlAnnotationParser.ParseSqlString(input);
        Assert.NotNull(annotations);
        Assert.Equal(1, annotations.Count);
        Assert.IsType<RouteSqlAnnotation>(annotations[0]);
    }

    [Theory]
    [InlineData("#Route(\"/test\")", "/test")]
    [InlineData("#Route( \"/te\\\"st\")", "/te\\\"st")]
    public void TestSequenceParsing(string input, string expectedPath)
    {
        List<ISqlAnnotation> annotations = SqlAnnotationParser.ParseSqlString(input);
        RouteSqlAnnotation? routeAnnotation = annotations.FirstOrDefault() as RouteSqlAnnotation;
        Assert.NotNull(routeAnnotation);
        if (routeAnnotation == null)
            return;

        Assert.Equal(expectedPath, routeAnnotation.Path);
    }

    [Theory]
    [InlineData("#Route(Path =\"/test\")", "/test")]
    [InlineData("#Route( Path= \"/te\\\"st\")", "/te\\\"st")]
    [InlineData("#Route(Path=\"/test\")", "/test")]
    [InlineData("#Route( Path =   \"/test\")", "/test")]
    public void TestNamedParsing(string input, string expectedPath)
    {
        List<ISqlAnnotation> annotations = SqlAnnotationParser.ParseSqlString(input);
        RouteSqlAnnotation? routeAnnotation = annotations.FirstOrDefault() as RouteSqlAnnotation;
        Assert.NotNull(routeAnnotation);
        if (routeAnnotation == null)
            return;

        Assert.Equal(expectedPath, routeAnnotation.Path);
    }
}