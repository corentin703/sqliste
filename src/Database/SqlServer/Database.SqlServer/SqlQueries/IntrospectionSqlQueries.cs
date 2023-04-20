namespace Sqliste.Database.SqlServer.SqlQueries;

public static class IntrospectionSqlQueries
{
    public static (string, object) GetProceduresQuery()
    {
        string query = "EXEC [sqliste].[p_web_procedures_get]";
        return (query, new {});
    }

    public static (string, object) GetProceduresArgumentsQuery(string procedureName)
    {
        var args = new
        {
            ProcedureName = procedureName,
        };

        string query = $"EXEC [sqliste].[p_web_procedure_params_get] @procedure_name = @{nameof(args.ProcedureName)}";
        return (query, args);
    }
}