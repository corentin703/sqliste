---
sidebar_position: 2
---

# HTTP Headers

HTTP headers allow both the client and the server to attach additional information to the request or the response.
Most of this information consists of parameters that can modify the behavior of the browser when included in an HTTP response.

:::note

The HTTP header of a request will be significantly different from that of a response, as the expressed parameters often have a different meaning depending on the context.
Some of them are only meaningful in a request or in a response.

:::

To learn more about standard header values and their usage, you can refer to this resource.

:::info

It is possible to define custom parameters, but keep in mind that the HTTP header is limited in size (maximum of 8192 bytes, including all parameters).

:::

## Reading

Reading a data from the header is similar to reading a cookie: we also have a key/value dictionary where the key corresponds to the name of the information, and the value is the data.

Example: reading an authentication token from the header (the _Authorization_ parameter)

```sql
#Route("/api/example/headers/read")
#HttpGet("ExampleReadHeader")
CREATE OR ALTER PROCEDURE [web].[p_example_header]
    @request_headers NVARCHAR(MAX) 
AS 
BEGIN
    DECLARE @auth_token NVARCHAR(MAX);
    DECLARE @username NVARCHAR(500);
    
    -- Retrieve the value from the "Authorization" parameter
    SET @auth_token = JSON_VALUE(@request_headers, '$.Authorization');

    -- Verify the token and retrieve the user...
    SELECT 
         '{ "message": "Welcome, ' + @username  + '!" }' AS [response_body]
        ,200 AS [response_status]
    ;
END
GO
```

## Writing

Unlike cookies, writing information to the header is also done using a key/value dictionary following the same logic as reading.

Example: we set the _Max-Age_ parameter so that our resource is cached in the browser and only requested again when it expires.

```sql
#Route("/api/example/headers/write")
#HttpGet("ExampleWriteHeader")
CREATE OR ALTER PROCEDURE [web].[p_example_header]
    @request_headers NVARCHAR(MAX) 
AS 
BEGIN
    DECLARE @push_notification_token NVARCHAR(MAX);
    
    -- Retrieve the token for receiving push notifications... 
    
    -- Set the "Max-Age" header
    SET @request_headers = JSON_MODIFY(@request_headers, '$."Max-Age"', 21600); -- 60 * 24 * 15 = 21600 seconds = 15 days
    
    SELECT 
         '{ "token": "' + @push_notification_token + '" }' AS [response_body]
        ,200 AS [response_status]
        ,@request_headers AS [request_headers]
    ;
END
GO
```
