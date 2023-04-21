using FluentAssertions;
using Sqliste.Core.Utils.Url;

namespace Core.UnitTests.Utils.Url;

public class UrlParamsParserTests
{

    [Fact]
    public void ExtractSingleParamsTest()
    {
        string uri = "/test/15";
        string template = "/test/{number}";

        Dictionary<string, string> urlParams = UrlParamsParser.ParseUrlParams(template, uri);
        urlParams.Should()
            .NotBeNull()
            .And
            .HaveCount(1);

        urlParams.ContainsKey("number").Should().BeTrue();
        string numberParam = urlParams["number"];
        numberParam.Should().Be("15");
    }

    [Fact]
    public void ExtractMultipleParamsTest()
    {
        string uri = "/test/15/superAction";
        string template = "/test/{number}/{action}";

        Dictionary<string, string> urlParams = UrlParamsParser.ParseUrlParams(template, uri);
        urlParams.Should()
            .NotBeNull()
            .And
            .HaveCount(2);

        urlParams.ContainsKey("number").Should().BeTrue();
        string numberParam = urlParams["number"];
        numberParam.Should().Be("15");

        urlParams.ContainsKey("number").Should().BeTrue();
        string actionParam = urlParams["action"];
        actionParam.Should().Be("superAction");
    }

    [Fact]
    public void ExtractWithOptionalParamsTest()
    {
        string uri = "/test/15";
        string template = "/test/{number}/{action?}";

        Dictionary<string, string> urlParams = UrlParamsParser.ParseUrlParams(template, uri);
        urlParams.Should()
            .NotBeNull()
            .And
            .HaveCount(1);

        urlParams.ContainsKey("number").Should().BeTrue();
        string numberParam = urlParams["number"];
        numberParam.Should().Be("15");

        uri = "/test/15/superAction";
        urlParams = UrlParamsParser.ParseUrlParams(template, uri);
        urlParams.Should()
            .NotBeNull()
            .And
            .HaveCount(2);

        urlParams.ContainsKey("action").Should().BeTrue();
        string actionParam = urlParams["action"];
        actionParam.Should().Be("superAction");
    }
}