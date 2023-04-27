using Sqliste.Core.Models.Http;

namespace Sqliste.Core.Contracts.Services;

public interface IRequestHandlerService
{
    public Task<HttpRequestModel> HandleRequestAsync(HttpRequestModel request, CancellationToken cancellationToken = default);
}