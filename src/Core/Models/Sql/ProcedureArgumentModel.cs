using DapperCodeFirstMappings.Attributes;
using Microsoft.OpenApi.Models;
using System.Data;

namespace Sqliste.Core.Models.Sql;

public class ProcedureArgumentModel
{
    public string Name { get; set; } = string.Empty;
    public string SqlDataType { get; set; } = string.Empty;
    public ParameterLocation Location { get; set; } = ParameterLocation.Query;
    public bool IsSystemParam { get; set; }
}