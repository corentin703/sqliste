SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [sqliste].[p_web_procedures_get] AS
BEGIN
    SELECT 
        'web' AS [schema], 
        [ROUTINE_NAME] AS [name], 
        [ROUTINE_DEFINITION] AS [content]
    FROM [INFORMATION_SCHEMA].[ROUTINES]
    WHERE [ROUTINE_SCHEMA] = 'web' AND [ROUTINE_TYPE] = 'PROCEDURE';
END
GO

CREATE OR ALTER PROCEDURE [sqliste].[p_web_procedure_params_get]
   @procedure_name NVARCHAR(MAX)
AS 
BEGIN
    SELECT --*,
        --  P.parameter_id AS [order]
         REPLACE(P.name, '@', '') AS [name]
        ,TYPE_NAME(P.user_type_id) AS [sql_data_type]
        -- ,P.is_output AS [is_output]
    FROM sys.objects AS SO
    INNER JOIN sys.parameters AS P ON SO.OBJECT_ID = P.OBJECT_ID
    WHERE SCHEMA_NAME(SCHEMA_ID) = 'web' AND SO.name = @procedure_name
    ORDER BY P.parameter_id
END
GO

CREATE OR ALTER   PROCEDURE [sqliste].[p_event_trigger_web_schema_update]
AS 
BEGIN
    INSERT INTO [sqliste].[app_events] ([type], [name])
    VALUES ('SYS', 'WebSchemaUpdate');
END
GO

CREATE OR ALTER PROCEDURE [sqliste].[pr_app_event_cleanup]
AS 
BEGIN
    DELETE FROM [sqliste].[app_events]
    WHERE [inserted_at] < DATEADD(hh, -1, GETDATE()); 
END
GO

