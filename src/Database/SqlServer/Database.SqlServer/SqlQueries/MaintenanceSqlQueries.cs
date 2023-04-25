namespace Sqliste.Database.SqlServer.SqlQueries;

public static class MaintenanceSqlQueries
{
    public static (string, object) GetAppEventTableCleaningProcedure()
    {
        string query = "EXEC [sqliste].[pr_app_event_cleanup]";
        return (query, new {});
    }
}