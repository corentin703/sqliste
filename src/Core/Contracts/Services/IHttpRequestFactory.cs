using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Pipeline;

namespace Sqliste.Core.Contracts.Services;

public interface IHttpModelsFactory
{
    public Task<PipelineBag> BuildRequestModelAsync(CancellationToken cancellationToken = default);
}