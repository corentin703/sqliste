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

    public static (string, object) GetOpenApiDocumentQuery()
    {
        string query = "EXEC [sqliste].[p_openapi_document_get]";
        return (query, new {});
    }

    public static (string, object) GetOpenApiTypeFromSqlQuery(string sqlType)
    {
        var args = new
        {
            SqlType = sqlType,
        };

        string query = $"EXEC [sqliste].[p_openapi_convert_type_sql_to_openapi] @sql_type = @{nameof(args.SqlType)}, @select = 1";
        return (query, args);
    }
}