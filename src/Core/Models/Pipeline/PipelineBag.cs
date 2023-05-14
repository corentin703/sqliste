using DapperCodeFirstMappings.Attributes;
using Sqliste.Core.Constants;
using Sqliste.Core.Models.Sql;

namespace Sqliste.Core.Models.Pipeline;

[DapperEntity]
public class PipelineBag
{
    public PipelineRequestBag Request { get; init; } = new();
    public PipelineResponseBag Response { get; set; } = new();

    public ProcedureModel? Procedure { get; init; }
    
    public SqlErrorModel? Error { get; set; }
}