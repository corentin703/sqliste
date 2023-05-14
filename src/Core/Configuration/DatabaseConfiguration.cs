namespace Sqliste.Core.Configuration;

public class DatabaseConfiguration
{
    public string ConnectionString { get; set; } = string.Empty;
    public MigrationConfiguration Migration { get; set; } = new();
}