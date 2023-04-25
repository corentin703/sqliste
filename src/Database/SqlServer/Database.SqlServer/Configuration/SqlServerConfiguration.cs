using Sqliste.Core.Configuration;

namespace Sqliste.Database.SqlServer.Configuration;

public class SqlServerConfiguration : DatabaseConfiguration
{
    // In minutes
    public int AppEventTableCleanInterval { get; set; } = 60;
}