---
sidebar_position: 7
---

# Error Handling

When an error occurs in a procedure during the pipeline, it interrupts the pipeline and returns a 500 (_INTERNAL SERVER ERROR_) error.

To enable error handling, you can take the _error_ standard parameter in the procedures of the pipeline.
When an error occurs, SQListe will look at the list of arguments in the procedures of the pipeline that have not been executed yet. If such an argument exists, it will take the first one and execute it.<br/>

If this procedure doesn't raise an error itself, it will be considered as handled.

The _error_ argument is a JSON object following the format below:
```json lines
{
  "message": "Message de l'erreur", // Cela peut contenir une chaine tout à fait arbitraire *
  "attributes": {
    "state": 1 // État de l'erreur remonté par SQL Server
  }
}
```

This mechanism allows you to intercept an error and modify the response accordingly.

:::info

When multiple procedures in the pipeline raise errors, the mechanism is executed each time.

:::

:::note About the _message_ field of the _error_ argument *

It is possible to pass a structured string (such as JSON) as a message in a RAISERROR statement.
You can retrieve it here and process the response accordingly.

:::

Practical example: a post-middleware catching all errors:

```sql
-- #Middleware(Order = 1000, After = true)
CREATE OR ALTER PROCEDURE [web].[p_middleware_catch_error]
    @request_body NVARCHAR(MAX) = NULL,
    @request_headers NVARCHAR(MAX) = NULL,
    @request_cookies NVARCHAR(MAX) = NULL,
    @request_path NVARCHAR(MAX) = NULL,
    @pipeline_storage NVARCHAR(MAX),
    @error NVARCHAR(MAX) = NULL
AS
BEGIN
    IF (@error IS NULL)
    BEGIN
        RETURN;
    END

    -- Retrieve the "message" property from the error JSON
    SET @error_message = JSON_VALUE(@error, '$.message');

    -- If it's not a JSON, we don't handle it (SQListe will return an HTTP 500)
    IF (ISJSON(@error_message) = 0)
    BEGIN
        RAISERROR(@error_message, 18, 1);
        RETURN;
    END
    
    DECLARE @error_message NVARCHAR(MAX);
    DECLARE @response_body NVARCHAR(MAX);
    DECLARE @response_headers NVARCHAR(MAX) = N'{ "Content-Type": "application/json" }';

    -- Parse the JSON and perform our processing
    DECLARE
        @message NVARCHAR(MAX),
        @status INT
    ;

    SELECT
        @message = [error_message],
        @status = [http_status]
    FROM OPENJSON(@error_message)
    WITH (
        [error_message] NVARCHAR(MAX),
        [http_status] INT
    );

    SET @response_body = (
        SELECT
            @message AS [message],
            @status AS [status]
        FOR JSON PATH
    );

    SELECT
         JSON_QUERY(@response_body, '$[0]') AS [response_body]
        ,@status AS [response_status]
        ,@response_headers AS [response_headers]
        ,0 AS [next]
    ;
END
GO
```

In this example, the message passed to RAISERROR is a JSON object on which we perform processing to alter the response appropriately.
