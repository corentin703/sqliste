SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- #Route("/api/books")
-- #HttpPost
CREATE PROCEDURE [web].[p_books_create] 
    @body NVARCHAR(MAX) = NULL,
    @headers NVARCHAR(MAX) = NULL,
    @cookies NVARCHAR(MAX) = NULL
AS 
BEGIN
    SELECT 
        @body AS [body]
    ;
END
GO

-- #Route("/api/books/{id}")
-- #HttpDelete
CREATE PROCEDURE [web].[p_books_delete] AS 
BEGIN
    SELECT 4;
END
GO

-- #Route("/api/books/{id}")
-- #HttpGet
-- #Accepts(Mime = "application/json")
-- #Produces(Mime = "application/json")
-- #Takes(Type = "book")
-- #Responds(Type = "book", Status = 200)
CREATE PROCEDURE [web].[p_books_details] 
    @id BIGINT,
    @body NVARCHAR(MAX) = NULL,
    @headers NVARCHAR(MAX) = NULL,
    @cookies NVARCHAR(MAX) = NULL,
    @name NVARCHAR(MAX) = NULL
AS 
BEGIN
    DECLARE @response_body NVARCHAR(MAX);
    DECLARE @response_headers NVARCHAR(MAX);

    SET @response_body = (
        SELECT 
             @id AS id
            ,@name AS name
            ,@body AS body
            ,@headers AS headers
            ,@cookies AS cookies
        FOR JSON PATH
    );

    SELECT @response_body = JSON_QUERY(@response_body, '$[0]');

    -- SET @response_headers = (
    --     SELECT 
    --          'text/json' AS [Content-Type]
    --         ,'SQListe' AS [Server]
    --     FOR JSON PATH
    -- );
    -- SELECT @response_headers = JSON_QUERY(@response_headers, '$[0]');
    
    SELECT 
         @response_body AS [body]
        ,200 AS [status]
        ,@response_headers AS [headers]
    ;
END
GO

-- #Route("/api/books")
-- #HttpGet
CREATE PROCEDURE [web].[p_books_read] AS 
BEGIN
    SELECT 4;
END
GO

-- #Route("/api/books/{id}")
-- #HttpPut
-- #HttpPatch
CREATE PROCEDURE [web].[p_books_update] 
    @id BIGINT
AS 
BEGIN
    SELECT 4;
END
GO

-- #Route("/api/identity")
-- #HttpPost
-- #Produces(Mime = "application/json")
CREATE   PROCEDURE [web].[p_identity_register] 
    @body NVARCHAR(MAX) = NULL,
    @headers NVARCHAR(MAX) = NULL,
    @cookies NVARCHAR(MAX) = NULL,
    @name NVARCHAR(MAX) = NULL
AS 
BEGIN
    DECLARE @response_body NVARCHAR(MAX);
    DECLARE @response_headers NVARCHAR(MAX);

    SET @response_body = (
        SELECT 
             @name AS name
            ,@body AS body
            ,@headers AS headers
            ,@cookies AS cookies
        FOR JSON PATH
    );

    SELECT @response_body = JSON_QUERY(@response_body, '$[0]');

    -- SET @response_headers = (
    --     SELECT 
    --          'text/json' AS [Content-Type]
    --         ,'SQListe' AS [Server]
    --     FOR JSON PATH
    -- );
    -- SELECT @response_headers = JSON_QUERY(@response_headers, '$[0]');
    
    SELECT 
         @response_body AS [Body]
        ,200 AS [Status]
        ,@response_headers AS [Headers]
    ;
END
GO

-- #Middleware(Order = 1, PathStarts = "/api/books")
CREATE   PROCEDURE [web].[p_middleware_identity]
    @body NVARCHAR(MAX) = NULL,
    @headers NVARCHAR(MAX) = NULL,
    @cookies NVARCHAR(MAX) = NULL,
    @route NVARCHAR(MAX) = NULL,
    @data_bag NVARCHAR(MAX)
AS
BEGIN
    PRINT 'auth';
    SELECT '{}' AS [body], 1 AS 'next'
END
GO

-- #Middleware(Order = 1, PathStarts = "/api/books", After = true)
CREATE   PROCEDURE [web].[p_middleware_catch_error]
    @body NVARCHAR(MAX) = NULL,
    @headers NVARCHAR(MAX) = NULL,
    @cookies NVARCHAR(MAX) = NULL,
    @route NVARCHAR(MAX) = NULL,
    @data_bag NVARCHAR(MAX),
    @error NVARCHAR(MAX),
    @error_attributes NVARCHAR(MAX) = NULL
AS
BEGIN
    DECLARE @response_body NVARCHAR(MAX);

    IF (@error IS NULL) 
    BEGIN
        RETURN;
    END

    SET @response_body = (
        SELECT 
            @error AS [error],
            @error_attributes  AS [error_attributes]
        FOR JSON PATH
    );

    SELECT @response_body = JSON_QUERY(@response_body, '$[0]');

    SELECT @response_body AS [body], 1 AS 'next'
END
GO

CREATE OR ALTER    PROCEDURE [sqliste].[p_openapi_components_get]
    @json NVARCHAR(MAX) OUT
AS
BEGIN
    DECLARE @type_def_json NVARCHAR(MAX);
    DECLARE @property_def_json NVARCHAR(MAX);

    EXEC [sqliste].[p_openapi_schema_json_create_components_def] @json = @json OUT;
    
        -- Book type
    EXEC [sqliste].[p_openapi_schema_json_create_type_def] @name = 'book', @title = 'Book', @description = 'A book', @json = @type_def_json OUT;
    
        ---- Title prop
    EXEC [sqliste].[p_openapi_schema_json_create_property_def] @name = 'tile', @type = 'string', @required = 1, @property_def_json = @property_def_json OUT;
    EXEC [sqliste].[p_openapi_schema_json_add_property_def] @property_def_json = @property_def_json, @type_def_json = @type_def_json OUT;
    
        ---- Abstract prop
    EXEC [sqliste].[p_openapi_schema_json_create_property_def] @name = 'abstract', @type = 'string', @required = 0, @property_def_json = @property_def_json OUT;
    EXEC [sqliste].[p_openapi_schema_json_add_property_def] @property_def_json = @property_def_json, @type_def_json = @type_def_json OUT;
    
        ---- Author prop
    EXEC [sqliste].[p_openapi_schema_json_create_property_def] @name = 'author', @type = 'string', @required = 0, @property_def_json = @property_def_json OUT;
    EXEC [sqliste].[p_openapi_schema_json_add_property_def] @property_def_json = @property_def_json, @type_def_json = @type_def_json OUT;
    
        ---- Tags prop
    EXEC [sqliste].[p_openapi_schema_json_create_property_ref] @name = 'tags', @type_name = 'bookTag', @property_def_json = @property_def_json OUT;
    EXEC [sqliste].[p_openapi_schema_json_create_array_property_def] @name = 'tags', @items_type_def_json = @property_def_json, @property_def_json = @property_def_json OUT;
    EXEC [sqliste].[p_openapi_schema_json_add_property_def] @property_def_json = @property_def_json, @type_def_json = @type_def_json OUT;
    
    EXEC [sqliste].[p_openapi_schema_json_add_type_def] @type_def_json = @type_def_json OUT, @json = @json OUT;
    
        -- Book tag type
    EXEC [sqliste].[p_openapi_schema_json_create_type_def] @name = 'bookTag', @title = 'Book Tag', @description = 'A tag for a book', @json = @type_def_json OUT;
    
        ---- Label prop
    EXEC [sqliste].[p_openapi_schema_json_create_property_def] @name = 'label', @type = 'string', @required = 1, @property_def_json = @property_def_json OUT;
    EXEC [sqliste].[p_openapi_schema_json_add_property_def] @property_def_json = @property_def_json, @type_def_json = @type_def_json OUT;
    
    EXEC [sqliste].[p_openapi_schema_json_add_type_def] @type_def_json = @type_def_json OUT, @json = @json OUT;
END
GO

CREATE OR ALTER    PROCEDURE [sqliste].[p_openapi_document_get]
AS
BEGIN
    DECLARE @json NVARCHAR(MAX);
    DECLARE @component_json NVARCHAR(MAX);
    DECLARE @info_json NVARCHAR(MAX) = N'{
        "title": "SQListe",
        "version": "1.0.0"
    }';

    EXEC [sqliste].[p_openapi_components_get] @json = @component_json OUT;

    SET @json = (
        SELECT 
            '3.0.0' AS [openapi]
            ,JSON_QUERY(@info_json) AS [info]
            ,JSON_QUERY(@component_json) AS [components]
            ,JSON_QUERY('{}') AS [paths]
        FOR JSON PATH
    );

    SET @json = JSON_QUERY(@json, '$[0]');
SELECT @json AS [document];
END
GO
