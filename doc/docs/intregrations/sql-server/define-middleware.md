---
sidebar_position: 4
---

# Define a Middleware

A middleware takes the form of a stored procedure located in the _web_ schema and annotated with ```#Middleware```.

This annotation has several parameters:
- After : Specifies whether it is a post-middleware or pre-middleware. By default, it is a pre-middleware.
- Order : Specifies the execution order of the middleware when there are multiple middlewares.
- PathStarts: Allows targeting a group of routes. For example, a middleware with a ```PathStart``` value of ```/api/books``` will only be executed if the corresponding controller for the request declares a route starting with ```/api/books```.

Similar to controllers, it is possible to alter the response state and interrupt processing.
The difference is that you don't have obligations regarding the response, as shown in the example below.

Example: a logging middleware

```sql
-- #Middleware(Order = 1, After = false)
CREATE OR ALTER PROCEDURE [web].[p_middleware_log_request_start]
    @request_path NVARCHAR(MAX)
AS
BEGIN 
    INSERT INTO app_logs ([level], [message])
    SELECT 
         'info' AS [level]
        ,'Starting request at route ' + @request_path AS [message]
    ;
END
GO
```

In some cases, it may be necessary to interrupt the execution of the procedure pipeline and return the response as is
(for example, if an anonymous user tries to access a protected resource).
To do this, simply return the standard parameter ```next``` with a value of 0, as shown in the following example.

Example: (very) simplified authentication control
```sql
-- #Middleware(Order = 2, After = false)
CREATE OR ALTER PROCEDURE [web].[p_middleware_authentication_control]
    @request_headers NVARCHAR(MAX)
AS
BEGIN 
    DECLARE @auth_token NVARCHAR(MAX);

    IF (JSON_VALUE(@request_headers, '$.Authorization') IS NULL)
    BEGIN
        SELECT 
             401 AS [response_status]
            ,0 AS [next]
        ;
    END
END
GO
```

In the above example, if no authentication token is provided by the client, a 401 status is returned, and the procedure pipeline is interrupted.<br/>
_In a real world application, you would need to check the validity of the token._

