namespace Sqliste.Core.Configuration;

public class DatabaseConfiguration
{
    public string ConnectionString { get; set; } = string.Empty;
    public string EntryPointSchemaName { get; set; } = "web";
}