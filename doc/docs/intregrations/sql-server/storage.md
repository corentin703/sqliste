---
sidebar_position: 5
---

# Storage

To simplify development, SQListe provides access to two storage locations for storing data for later use.

The recommended format for these storage locations is JSON. They are initialized with an empty JSON object ```{}``` and are an integral part of the response state.

:::note

The content of these storages is only initialized by SQListe.<br/>
Since they are not subject to any specific processing, you can define a different format by overwriting the default value.

:::

:::info

The client does not have access to the content of these storage locations; they remain internal to SQListe.<br/>
Therefore, you can safely store sensitive data in them.

:::

## Session

The first storage location provides access to the HTTP session.<br/>
The session is a storage location typically resolved from a token stored in an HTTP cookie (which is our case).
Its lifespan is 20 minutes by default (configurable in the _appsettings.json_ file).

Example usage:

```sql
#Route("/api/example/session")
#HttpGet("ExampleSession")
CREATE OR ALTER PROCEDURE [web].[p_example_session]
    @session NVARCHAR(MAX) 
AS 
BEGIN
    IF (JSON_VALUE(@session, '$.fancyCalculation' IS NULL))
    BEGIN
        DECLARE @fancy_calculation INT;
        SET @fancy_calculation = 1 + 1;
    
        SET @session = JSON_MODIFY(@session, '$.fancyCalculation', @fancy_calculation);
    END
    
    -- Other processing...

    SELECT 
         @session AS [session]
    ;
END
GO
```

The very fancy and expensive calculation in the example above will only be executed if the result is not present in the session.

:::info

The HTTP session is only modified at the end of the pipeline processing.

:::

:::note Initialization

The session (and the corresponding cookie) will only be initialized if it is requested by a procedure.

:::

## Pipeline Storage

To transmit information within a pipeline, SQListe provides a storage with a lifespan of one request.

Example usage:
```sql
-- #Middleware(Order = 1, After = false)
CREATE OR ALTER PROCEDURE [web].[p_middleware_log_request_start]
    @request_headers NVARCHAR(MAX),
    @pipeline_storage NVARCHAR(MAX) 
AS
BEGIN 
    DECLARE @username NVARCHAR(50);
    
    -- Read the token, verify and retrieve the user...
    
    SET @pipeline_storage = JSON_MODIFY(@pipeline_storage, '$.username', @username);

    SELECT 
         @pipeline_storage AS [pipeline_storage]
    ;
END
GO
```

In this example, we retrieve the username and insert it into the pipeline storage.<br/>
This allows a subsequent middleware or controller to access this data without performing additional processing.

:::note Usage

These two storages are used in the same way, with the only difference being their lifespan.<br/>
Therefore, the session is more appropriate for storing data that is expensive to compute or cache, while the pipeline storage
is more suitable for storing data that changes frequently and can be obtained quickly.

:::
