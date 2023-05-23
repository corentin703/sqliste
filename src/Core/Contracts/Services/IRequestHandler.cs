using Sqliste.Core.Models.Http;
using Sqliste.Core.Models.Pipeline;

namespace Sqliste.Core.Contracts.Services;

public interface IRequestHandler
{
    public Task<PipelineBag> HandleRequestAsync(PipelineBag pipeline, CancellationToken cancellationToken = default);
}