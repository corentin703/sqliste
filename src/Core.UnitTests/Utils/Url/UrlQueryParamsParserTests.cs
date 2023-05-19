using FluentAssertions;
using Sqliste.Core.Utils.Uri;

namespace Core.UnitTests.Utils.Url;

public class UrlQueryParamsParserTests
{
    [Fact]
    public void ExtractSingleParamsTest()
    {
        string uri = "/test?number=15";

        Dictionary<string, string> urlParams = UriQueryParamsParser.ParseQueryParams(uri);
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
        string uri = "/test/?number=15&action=superAction";

        Dictionary<string, string> urlParams = UriQueryParamsParser.ParseQueryParams(uri);
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
}