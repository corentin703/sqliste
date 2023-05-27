SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [sqliste].[p_event_trigger_web_schema_update]
AS
BEGIN
    INSERT INTO [sqliste].[app_events] ([type], [name])
    VALUES ('SYS', 'WebSchemaUpdate');
END
GO
