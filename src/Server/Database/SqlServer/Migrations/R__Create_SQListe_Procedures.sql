SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DECLARE @current_db_name NVARCHAR(MAX);
DECLARE @query_temp NVARCHAR(MAX);

SET @current_db_name = DB_NAME();

-- SET @query_temp = 'ALTER DATABASE ' + @current_db_name + ' SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE';
-- EXEC(@query_temp);

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE [name] = N'sqliste' )
    EXEC('CREATE SCHEMA [sqliste]');
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE [name] = N'web' )
    EXEC('CREATE SCHEMA [web]');
GO

CREATE OR ALTER PROCEDURE [sqliste].[p_web_procedures_get] AS
BEGIN
    SELECT 
        'web' AS 'Schema', 
        [ROUTINE_NAME] AS [Name], 
        [ROUTINE_DEFINITION] AS [Content]
    FROM [INFORMATION_SCHEMA].[ROUTINES]
    WHERE [ROUTINE_SCHEMA] = 'web' AND [ROUTINE_TYPE] = 'PROCEDURE';
END
GO

CREATE OR ALTER PROCEDURE [sqliste].[p_web_procedure_params_get]
    @procedure_name NVARCHAR(MAX)
AS 
BEGIN
    SELECT 
         P.parameter_id AS [Order]
        ,REPLACE(P.name, '@', '') AS [Name]
        ,TYPE_NAME(P.user_type_id) AS [SqlDataType]
    FROM sys.objects AS SO
    INNER JOIN sys.parameters AS P ON SO.OBJECT_ID = P.OBJECT_ID
    WHERE SCHEMA_NAME(SCHEMA_ID) = 'web' AND SO.name = @procedure_name
    ORDER BY P.parameter_id
END
GO

CREATE OR ALTER   PROCEDURE [sqliste].[p_event_trigger_web_schema_update]
AS 
BEGIN
    INSERT INTO [sqliste].[app_events] ([Type], [Name])
    VALUES ('SYS', 'WebSchemaUpdate');
END
GO

CREATE OR ALTER   PROCEDURE [sqliste].[pr_app_event_cleanup]
AS 
BEGIN
    DELETE FROM [sqliste].[app_events]
    WHERE [InsertedAt] < DATEADD(hh, -1, GETDATE()); 
END
GO
