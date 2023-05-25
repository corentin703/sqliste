using DapperCodeFirstMappings.Attributes;

namespace Sqliste.Database.SqlServer.Models;

[DapperEntity]
internal class SqlServerProcedureArgumentModel
{
    [DapperColumn("name")]
    public string Name { get; set; } = string.Empty;
    
    [DapperColumn("sql_data_type")]
    public string SqlDataType { get; set; } = string.Empty;

    [DapperColumn("is_output")]
    public bool IsOutput { get; set; }
}