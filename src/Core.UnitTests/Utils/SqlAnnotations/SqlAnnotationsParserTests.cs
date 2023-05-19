using FluentAssertions;
using Sqliste.Core.Contracts;
using Sqliste.Core.Models.SqlAnnotations;
using Sqliste.Core.Utils.SqlAnnotations;

namespace Core.UnitTests.Utils.SqlAnnotations;

public class SqlAnnotationsParserTests
{
    [Theory]
    [InlineData("#Route", 1)]
    [InlineData("#Route(\"/test\")", 1)]
    [InlineData("#Route(\"/test\") \n #Route(\"/test\")", 2)]
    [InlineData("-- Some comments \n -- #Route(\"/test\") \n -- #Route(\"/test\")", 2)]
    //[InlineData("#Route( \"/te\\\"st\", \"ss\")")]
    public void TestInstantiation(string input, int length)
    {
        List<ISqlAnnotation> annotations = SqlAnnotationParser.ParseSqlString(input);

        annotations.Should().NotBeNull();
        annotations.Count.Should().Be(length);

        annotations[0].Should().BeOfType<RouteSqlAnnotation>();
    }

    [Theory]
    [InlineData("#Route(\"/test\")", "/test")]
    [InlineData("#Route( \"/te\\\"st\")", "/te\\\"st")]
    public void TestSequenceParsing(string input, string expectedPath)
    {
        List<ISqlAnnotation> annotations = SqlAnnotationParser.ParseSqlString(input);
        RouteSqlAnnotation? routeAnnotation = annotations.FirstOrDefault() as RouteSqlAnnotation;
        
        routeAnnotation.Should().NotBeNull();
        routeAnnotation?.Path.Should().Be(expectedPath);
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
        
        routeAnnotation.Should()
            .NotBeNull();
        
        routeAnnotation?.Path.Should().Be(expectedPath);
    }
}