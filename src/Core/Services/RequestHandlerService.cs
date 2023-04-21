using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Requests;
using Sqliste.Core.Models.Response;

namespace Sqliste.Core.Services;

public class RequestHandlerService : IRequestHandlerService
{
    public Task<HttpResponseModel> HandleRequestAsync(HttpRequestModel request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}