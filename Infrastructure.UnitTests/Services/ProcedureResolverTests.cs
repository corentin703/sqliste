using FluentAssertions;
using Microsoft.OpenApi.Models;
using Moq;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Sql;
using Sqliste.Infrastructure.Services;

namespace Infrastructure.UnitTests.Services;

public class ProcedureResolverTests
{
    private ProcedureResolver _procedureResolver;

    public ProcedureResolverTests()
    {
        Mock<ISqlisteIntrospectionService> fakeIntrospectionService = new();

        DatabaseIntrospectionModel fakeIntrospection = new()
        {
            Endpoints = new List<ProcedureModel>()
            {
                new ProcedureModel()
                {
                      Name = "p_test",
                      Route = "/api/test",
                      RoutePattern = "^/api/test$",
                      Operations = new HttpOperationModel[]
                      {
                          new()
                          {
                              Method = HttpMethod.Get,
                          },
                          new()
                          {
                              Method = HttpMethod.Post,
                          },
                      },
                },
                new ProcedureModel()
                {
                    Name = "p_test_params_number",
                    Route = "/api/test/number/{number}",
                    RoutePattern = @"^/api/test/number/(?<number>\d+)$",
                    Operations = new HttpOperationModel[]
                    {
                        new()
                        {
                            Method = HttpMethod.Get,
                        },
                        new()
                        {
                            Method = HttpMethod.Patch,
                        },
                    },
                    UriParams = new List<HttpUriParam>()
                    {
                        new HttpUriParam()
                        {
                            Name = "number",
                            IsRequired = false,
                            Location = ParameterLocation.Path,
                        },
                    },
                },
                new ProcedureModel()
                {
                    Name = "p_test_params_string",
                    Route = "/api/test/string/{string}",
                    RoutePattern = @"^/api/test/string/(?<string>\w+)$",
                    Operations = new HttpOperationModel[]
                    {
                        new()
                        {
                            Method = HttpMethod.Delete,
                        },
                        new()
                        {
                            Method = HttpMethod.Post,
                        },
                    },
                    UriParams = new List<HttpUriParam>()
                    {
                        new HttpUriParam()
                        {
                            Name = "string",
                            IsRequired = false,
                            Location = ParameterLocation.Path,
                        },
                    },
                },
                new ProcedureModel()
                {
                    Name = "p_test_params_number_optional",
                    Route = "/api/test/number_opt/{number}",
                    RoutePattern = @"^/api/test/number_opt/?(?<number>\d+)?$",
                    Operations = new HttpOperationModel[]
                    {
                        new()
                        {
                            Method = HttpMethod.Put,
                        },
                        new()
                        {
                            Method = HttpMethod.Get,
                        },
                    },
                    UriParams = new List<HttpUriParam>()
                    {
                        new HttpUriParam()
                        {
                            Name = "number",
                            IsRequired = false,
                            Location = ParameterLocation.Path,
                        },
                    },
                },
                new ProcedureModel()
                {
                    Name = "p_test_params_string_optional",
                    Route = "/api/test/string_opt/{string}",
                    RoutePattern = @"^/api/test/string_opt/?(?<string>\w+)?$",
                    Operations = new HttpOperationModel[]
                    {
                        new()
                        {
                            Method = HttpMethod.Put,
                        },
                        new()
                        {
                            Method = HttpMethod.Delete,
                        },
                    },
                    UriParams = new List<HttpUriParam>()
                    {
                        new HttpUriParam()
                        {
                            Name = "string",
                            IsRequired = false,
                            Location = ParameterLocation.Path,
                        },
                    },
                },
            },
        };

        fakeIntrospectionService.Setup(service => 
            service.IntrospectAsync(default)
        ).Returns(Task.FromResult(fakeIntrospection));

        _procedureResolver = new ProcedureResolver(fakeIntrospectionService.Object);
    }

    private HttpMethod GetMethodFromString(string method)
    {
        return method.ToLower() switch
        {
            "get" => HttpMethod.Get,
            "post" => HttpMethod.Post,
            "put" => HttpMethod.Put,
            "patch" => HttpMethod.Patch,
            "delete" => HttpMethod.Delete,
            _ => HttpMethod.Get,
        };
    }
    
    [Theory]
    [InlineData("/api/test", "get", "p_test")]
    [InlineData("/api/test/number/15", "patch", "p_test_params_number")]
    [InlineData("/api/test/string/sample_string", "post", "p_test_params_string")]
    [InlineData("/api/test/string_opt/sample_string", "put", "p_test_params_string_optional")]
    [InlineData("/api/test/string_opt", "delete", "p_test_params_string_optional")]
    [InlineData("/api/test/number_opt/15", "get", "p_test_params_number_optional")]
    [InlineData("/api/test/number_opt", "put", "p_test_params_number_optional")]
    public async Task TestProcedureResolutionAsync(string route, string method, string expectedProcedureName)
    {
        ProcedureModel? procedure = await _procedureResolver.ResolveProcedureAsync(route, GetMethodFromString(method));
        procedure.Should().NotBeNull();
        procedure?.Name.Should().Be(expectedProcedureName);
    }
}