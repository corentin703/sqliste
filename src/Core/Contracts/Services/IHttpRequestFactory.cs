using Sqliste.Core.Models.Http;

namespace Sqliste.Core.Contracts.Services;

public interface IHttpModelsFactory
{
    public Task<HttpRequestModel> BuildRequestModelAsync(CancellationToken cancellationToken = default);
}