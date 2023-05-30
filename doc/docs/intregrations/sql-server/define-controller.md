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

:::

## Consuming a Form

Some Content-Type are used to handle form data (especially for the `<form />` tag in HTML).
In such cases, the Content-Type will be either `application/x-www-form-urlencoded` or `multipart/form-data`.

When a request of this type is sent to SQListe, it will not inject the data into the _request_body_ parameter but into the _request_form_ parameter.

The content of this parameter will be a JSON in the following format:
```json lines
[
  {
    "name": "field name",
    "value": "field value" // Always a string type
  },
  {
    "name": "second field name",
    "value": "second field value" // Always a string type
  },
  // ...
]
```

:::info

When the request_form parameter is not equal to NULL, the request_body parameter is.

:::

### Retrieving a File

Form requests allow direct file uploads without any specific encoding (such as base64).<br/>
When an element corresponds to a file, SQListe will inject a JSON object in the _request_form_ parameter for the respective field in the following format:
```json lines
[
  {
    "name": "name of a file field",
    "headers": {
      "Content-Type": "image/png", // MIME type corresponding to the file
      // ...
    },
    "length": 128000, // File size (in bytes)
    "contentType": "image/png", // Shortcut to access the "Content-Type" header of the file
    "contentDisposition": "form-data; name=\"fieldName\"; filename=\"filename.png\"", // Shortcut to access the "Content-Disposition" header of the file
    "fileName": "filename.png" // File name extracted from "Content-Disposition"
  },
  {
    "name": "name of a regular field",
    "value": "value of the regular field" // Always a string type
  },
  // ...
]
```

To access the content of the file, you need to retrieve a procedure parameter whose name follows the format
`request_form_file_<nomDuChamp>` and of type VARBINARY.

Example: Let's retrieve a form to save a music album with its cover photo.
```sql
-- #Route("/api/sampleForm")
-- #HttpPost("sampleForm")
CREATE OR ALTER PROCEDURE [web].[p_sample_form] 
    @request_form NVARCHAR(MAX), -- The user-entered information will be stored here
    @request_form_file_cover VARBINARY(MAX) -- We retrieve the cover as binary data, as sent by the user
AS 
BEGIN
    DECLARE @form_data TABLE (
         [name] NVARCHAR(500)
        ,[value] NVARCHAR(500)
        ,[file_name] NVARCHAR(500)
        ,[length] BIGINT
    );
    
    INSERT INTO @form_data
    SELECT 
         [name]
        ,[value]
        ,[fileName] AS [file_name]
        ,[length]
    FROM OPENJSON(@request_form)
    WITH (
         [name] NVARCHAR(500)
        ,[value] NVARCHAR(500)
        ,[fileName] NVARCHAR(500)
        ,[length] BIGINT
    );
    
    DECLARE @artist NVARCHAR(1000);
    DECLARE @title NVARCHAR(1000);
    DECLARE @cover_file_name NVARCHAR(1000);
    DECLARE @cover_length BIGINT;
    
    SELECT TOP 1 @artist = [value] FROM @form_data
    WHERE [name] = 'artist';
    
    SELECT TOP 1 @title = [value] FROM @form_data
    WHERE [name] = 'title';
    
    SELECT TOP 1 
         @cover_file_name = [file_name] 
        ,@cover_length = [length] / 1000 -- Obtaining the result in KB 
    FROM @form_data
    WHERE [name] = 'cover';
    
    -- Checking the image size
    IF (@cover_length > 500)
    BEGIN
        SELECT
             '{ "message": "The image you provided must be smaller than 500KB" }' AS [response_body]
            ,'application/json' AS [response_content_type] 
            ,413 AS [response_status] -- 413 = Payload Too Large
        ;
    END

    INSERT INTO [dbo].[albums] ([artist], [title], [cover], [cover_file_name])
    VALUES (
         @artist
        ,@title
        ,@request_form_file_cover
        ,@cover_file_name
    );
    
    SELECT 
        204 AS [response_status]
    ;
END
GO
```

:::caution

Make sure to check the file sizes that you archive.<br/>
The transfer of large files **is not** guaranteed.

:::

## File Download

To send files to the user, SQListe provides 3 output parameters:
- _response_file_ : used to define the content of the returned file (type _VARBINARY_)
- _response_file_name_ : indicates the file name (type _NVARCHAR_)
- _response_file_inline_ : _BIT_ to open the file in the browser's viewer (if available, otherwise it will be downloaded directly). False by default.

Example:
```sql
-- #Route("/api/sampleFileDownload")
-- #HttpPost("sampleFileDownload")
CREATE OR ALTER PROCEDURE [web].[p_sample_file_download] 
AS 
BEGIN
    DECLARE @file VARBINARY(MAX);
    DECLARE @file_name NVARCHAR(255);
    DECLARE @file_mime NVARCHAR(255);
    DECLARE @file_inline BIT = 0;
    
    -- Retrieving the file and its information
    -- ...

    -- If it's a PDF, request to display it in the browser, otherwise download directly
    IF (@file_mime = 'application/pdf')
        SET @file_inline = 1;

    SELECT 
         @file AS [response_file]
        ,@file_name AS [response_file_name]
        ,@file_inline AS [response_file_inline]
        ,@file_mime AS [response_content_type]
        ,200 AS [response_status]
    ;
END
GO
```

## Applying Changes

When you modify the annotations of a procedure, you need to notify SQListe that it needs to update its introspection.<br/>
To do this, execute the `[sqliste].[p_event_trigger_web_schema_update]` procedure.
