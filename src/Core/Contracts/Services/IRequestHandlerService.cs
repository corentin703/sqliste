using Sqliste.Core.Models.Http;

namespace Sqliste.Core.Contracts.Services;

public interface IRequestHandlerService
{
    public Task<HttpResponseModel> HandleRequestAsync(HttpRequestModel request, CancellationToken cancellationToken = default);
}