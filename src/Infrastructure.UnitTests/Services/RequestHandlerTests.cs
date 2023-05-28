using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Pipeline;
using Sqliste.Core.Models.Sql;
using Sqliste.Database.Common.Contracts.Services;
using Sqliste.Infrastructure.Services;

namespace Infrastructure.UnitTests.Services;

public class RequestHandlerTests
{
    private readonly Mock<IIntrospectionService> _introspectionServiceMock;
    private readonly Mock<IDatabaseGateway> _databaseGatewayMock;

    private DatabaseIntrospectionModel _introspectionModel = new();
    private readonly IRequestHandler _requestHandler;
    
    public RequestHandlerTests()
    {
        _introspectionServiceMock = new Mock<IIntrospectionService>();

        _introspectionServiceMock
            .Setup(service => service.IntrospectAsync(default))
            .ReturnsAsync(() => _introspectionModel);

        _databaseGatewayMock = new Mock<IDatabaseGateway>();
        
        _requestHandler = new RequestHandler(
            introspectionService: _introspectionServiceMock.Object,
            logger: new NullLogger<RequestHandler>(),
            sessionAccessor: Mock.Of<IDatabaseSessionAccessor>(),
            databaseGateway: _databaseGatewayMock.Object,
            parametersResolver: Mock.Of<IParametersResolver>()
        );
    }

    [Fact]
    public async Task TestProcedureExecutionAsync()
    {
        ProcedureModel procedureToRun = new()
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
            },
        };
        
        _introspectionModel.Endpoints = new List<ProcedureModel>()
        {
            procedureToRun,
        };

        _databaseGatewayMock.Setup(service => 
            service.ExecProcedureAsync(
                It.IsAny<PipelineRequestBag>(),
                It.Is<ProcedureModel>(it => it.Name == procedureToRun.Name),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(new PipelineResponseBag());
        
        PipelineBag pipelineBag = new PipelineBag()
        {
            Procedure = procedureToRun,
            Request = new PipelineRequestBag()
            {
                Path = "/api/test",
            },
        };
        await _requestHandler.HandleRequestAsync(pipelineBag);
        
        _databaseGatewayMock.Verify(service => 
            service.ExecProcedureAsync(
                It.IsAny<PipelineRequestBag>(),
                It.Is<ProcedureModel>(it => it.Name == procedureToRun.Name),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once()
        );
    }
    
    [Fact]
    public async Task TestMiddlewareExecutionAsync()
    {
        ProcedureModel beforeMiddleware1 = new()
        {
            Name = "p_test_before_middleware1",
            Route = "/api/test",
            RoutePattern = "^/api/test$",
        };
        
        _databaseGatewayMock.Setup(service => 
            service.ExecProcedureAsync(
                It.IsAny<PipelineRequestBag>(),
                It.Is<ProcedureModel>(it => it.Name == beforeMiddleware1.Name),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(new PipelineResponseBag());
        
        ProcedureModel beforeMiddleware2 = new()
        {
            Name = "p_test_before_middleware2",
            Route = "/api/none",
            RoutePattern = "^/api/none$",
        };
        
        _databaseGatewayMock.Setup(service => 
            service.ExecProcedureAsync(
                It.IsAny<PipelineRequestBag>(),
                It.Is<ProcedureModel>(it => it.Name == beforeMiddleware2.Name),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(new PipelineResponseBag());
        
        ProcedureModel beforeMiddleware3 = new()
        {
            Name = "p_test_before_middleware3",
            Route = "/api",
            RoutePattern = "^/api$",
        };
        
        _databaseGatewayMock.Setup(service => 
            service.ExecProcedureAsync(
                It.IsAny<PipelineRequestBag>(),
                It.Is<ProcedureModel>(it => it.Name == beforeMiddleware3.Name),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(new PipelineResponseBag());
        
        _introspectionModel.BeforeMiddlewares = new List<ProcedureModel>()
        {
            beforeMiddleware1,
            beforeMiddleware2,
            beforeMiddleware3,
        };
        
        ProcedureModel procedureToRun = new()
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
            },
        };
        
        _databaseGatewayMock.Setup(service => 
            service.ExecProcedureAsync(
                It.IsAny<PipelineRequestBag>(),
                It.Is<ProcedureModel>(it => it.Name == procedureToRun.Name),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(new PipelineResponseBag());
        
        _introspectionModel.Endpoints = new List<ProcedureModel>()
        {
            procedureToRun,
        };

        ProcedureModel afterMiddleware1 = new()
        {
            Name = "p_test_after_middleware1",
            Route = "/api",
            RoutePattern = "^/api",
        };
        
        _databaseGatewayMock.Setup(service => 
            service.ExecProcedureAsync(
                It.IsAny<PipelineRequestBag>(),
                It.Is<ProcedureModel>(it => it.Name == afterMiddleware1.Name),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(new PipelineResponseBag());
        
        ProcedureModel afterMiddleware2 = new()
        {
            Name = "p_test_after_middleware2",
            Route = "/api/fail",
            RoutePattern = "^/api/fail$",
        };

        _databaseGatewayMock.Setup(service => 
            service.ExecProcedureAsync(
                It.IsAny<PipelineRequestBag>(),
                It.Is<ProcedureModel>(it => it.Name == afterMiddleware2.Name),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(new PipelineResponseBag());
        
        ProcedureModel afterMiddleware3 = new()
        {
            Name = "p_test_after_middleware3",
            Route = "/api",
            RoutePattern = "^/api",
        };

        _databaseGatewayMock.Setup(service => 
            service.ExecProcedureAsync(
                It.IsAny<PipelineRequestBag>(),
                It.Is<ProcedureModel>(it => it.Name == afterMiddleware3.Name),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(new PipelineResponseBag());

        _introspectionModel.AfterMiddlewares = new List<ProcedureModel>()
        {
            afterMiddleware1,
            afterMiddleware2,
            afterMiddleware3,
        };
        
        PipelineBag pipelineBag = new PipelineBag()
        {
            Procedure = procedureToRun,
            Request = new PipelineRequestBag()
            {
                Path = "/api/test",
            },
        };
        await _requestHandler.HandleRequestAsync(pipelineBag);
        
        _databaseGatewayMock.Verify(service => 
                service.ExecProcedureAsync(
                    It.IsAny<PipelineRequestBag>(),
                    It.Is<ProcedureModel>(it => it.Name == beforeMiddleware1.Name),
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once()
        );
        
        _databaseGatewayMock.Verify(service => 
                service.ExecProcedureAsync(
                    It.IsAny<PipelineRequestBag>(),
                    It.Is<ProcedureModel>(it => it.Name == beforeMiddleware2.Name),
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Never
        );
        
        _databaseGatewayMock.Verify(service => 
                service.ExecProcedureAsync(
                    It.IsAny<PipelineRequestBag>(),
                    It.Is<ProcedureModel>(it => it.Name == beforeMiddleware3.Name),
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once()
        );
        
        _databaseGatewayMock.Verify(service => 
                service.ExecProcedureAsync(
                    It.IsAny<PipelineRequestBag>(),
                    It.Is<ProcedureModel>(it => it.Name == procedureToRun.Name),
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once()
        );
        
        _databaseGatewayMock.Verify(service => 
                service.ExecProcedureAsync(
                    It.IsAny<PipelineRequestBag>(),
                    It.Is<ProcedureModel>(it => it.Name == afterMiddleware1.Name),
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once()
        );
        
        _databaseGatewayMock.Verify(service => 
                service.ExecProcedureAsync(
                    It.IsAny<PipelineRequestBag>(),
                    It.Is<ProcedureModel>(it => it.Name == afterMiddleware2.Name),
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Never
        );
        
        _databaseGatewayMock.Verify(service => 
                service.ExecProcedureAsync(
                    It.IsAny<PipelineRequestBag>(),
                    It.Is<ProcedureModel>(it => it.Name == afterMiddleware3.Name),
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once()
        );
    }
}