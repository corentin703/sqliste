namespace Sqliste.Core.Models.Sql;

public class DatabaseIntrospectionModel
{
    public List<ProcedureModel> Endpoints { get; set; } = new();
    public List<ProcedureModel> BeforeMiddlewares { get; set; } = new();
    public List<ProcedureModel> AfterMiddlewares { get; set; } = new();
}