---
sidebar_position: 3
---

# Set up a Controller

A controller takes the form of a stored procedure located in the `web` schema.

Here is an example of a basic controller:
```sql
-- #Route("/api/helloWorld")
-- #HttpGet("HelloWorld")
CREATE OR ALTER PROCEDURE [web].[p_hello_world] 
AS 
BEGIN
    SELECT 
         'Greetings, everyone!' AS [response_body] -- Returning the response body
        ,'text/plain' AS [response_content_type] -- MIME type of the response body
        ,200 AS [response_status] -- HTTP status of the response
    ;
END
GO
```

## URI parameters

A URI parameter is an argument provided as follows in a URI: ```/api/exemple?name=Corentin```<br/>
To retrieve its value, simply include it as an argument in the stored procedure:
```sql
-- Define the 'name' parameter in the route using curly braces
-- #Route("/api/helloWorld")
-- #HttpGet("HelloWorld")
CREATE OR ALTER PROCEDURE [web].[p_hello_world] 
    @name NVARCHAR(100) = NULL -- Retrieve the 'name' URI parameter
AS 
BEGIN
    DECLARE @response_body NVARCHAR(MAX);
    
    IF (@name IS NULL)
        SET @response_body = 'Greetings, everyone!';
    ELSE    
        SELECT @response_body = 'Greetings, ' + @name + '!';
        
    SELECT 
         @response_body AS [response_body]
        ,'text/plain' AS [response_content_type]
        ,200 AS [response_status]
    ;
END
GO
```

A URI parameter is always considered optional and should be of type _NVARCHAR_ (you can cast it to another type within the stored procedure if needed).

:::caution

It is recommended to provide a default value for URI parameters and optional route parameters in the stored procedure arguments. If no default value is provided and the parameter is not provided in the URI, the database will not be able to execute the procedure and will return an error.

:::

## Route Parameters

Like most web frameworks, SQListe allows you to define route patterns. This allows you to pass arguments directly in the route instead of using URI parameters or request body.

Exemple:
```sql
-- Define the 'name' parameter in the route using curly braces
-- #Route("/api/helloWorld/{name}")
-- #HttpGet("HelloWorld")
CREATE OR ALTER PROCEDURE [web].[p_hello_world] 
    @name NVARCHAR(100) -- Retrieve the parameter with the same name as the stored procedure argument
AS 
BEGIN
    DECLARE @response_body NVARCHAR(MAX);
    
    SELECT @response_body = 'Greetings, ' + @name + '!';
        
    SELECT 
         @response_body AS [response_body]
        ,'text/plain' AS [response_content_type]
        ,200 AS [response_status]
    ;
END
GO
```

:::note

It is possible to define multiple parameters in the route with different names.

Like URI parameters, route parameters will be injected as NVARCHAR type.

:::

:::caution

If a parameter name matches a reserved keyword, the reserved keyword takes precedence.

:::

In the above example, the parameter is considered required: if it is not provided, SQListe will not match this route.
To define an optional parameter, you can do the following:
```sql
-- Define the 'name' parameter in the route using curly braces
-- #Route("/api/helloWorld/{name?}")
-- #HttpGet("HelloWorld")
CREATE OR ALTER PROCEDURE [web].[p_hello_world] 
    @name NVARCHAR(100) = NULL -- Retrieve the parameter with the same name as the stored procedure argument, with a default value of NULL (useful if the parameter is not provided).
AS 
BEGIN
    DECLARE @response_body NVARCHAR(MAX);
    
    IF (@name IS NULL)
        SET @response_body = 'Greetings, everyone!';
    ELSE    
        SELECT @response_body = 'Greetings, ' + @name + '!';
        
    SELECT 
         @response_body AS [response_body]
        ,'text/plain' AS [response_content_type]
        ,200 AS [response_status]
    ;
END
GO
```

An optional parameter must be placed at the end of the route.
For example:
- ```/api/say/hello/{name?}/{age?}``` => Valid.
- ```/api/say/hello/{name}/withAge/{age?}``` => Valid.
- ```/api/say/hello/{name?}/withAge/{age?}``` => Invalid: correct functionality cannot be guaranteed.

When dealing with multiple optional parameters, it is often better to use URI parameters as described earlier.

:::warning Limitations

The length of a URI is limited to a maximum of 2048 characters. Problems may occur when reading parameters that exceed this limit.

_Some servers allow increasing this limit, but it is important to check whether the HTTP client/browser supports this increase._
