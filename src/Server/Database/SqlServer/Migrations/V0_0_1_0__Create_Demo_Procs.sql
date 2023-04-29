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
