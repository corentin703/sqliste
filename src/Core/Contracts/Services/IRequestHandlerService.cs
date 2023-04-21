using Sqliste.Core.Models.Requests;
using Sqliste.Core.Models.Response;

namespace Sqliste.Core.Contracts.Services;

public interface IRequestHandlerService
{
    public Task<HttpResponseModel> HandleRequestAsync(HttpRequestModel request, CancellationToken cancellationToken = default);
}