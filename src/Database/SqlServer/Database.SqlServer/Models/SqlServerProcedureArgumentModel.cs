using DapperCodeFirstMappings.Attributes;
using Microsoft.OpenApi.Models;

namespace Sqliste.Database.SqlServer.Models;

[DapperEntity]
public class SqlServerProcedureArgumentModel
{
    [DapperColumn("name")]
    public string Name { get; set; } = string.Empty;
    
    [DapperColumn("sql_data_type")]
    public string SqlDataType { get; set; } = string.Empty;

    [DapperColumn("is_output")]
    public bool IsOutput { get; set; }
}