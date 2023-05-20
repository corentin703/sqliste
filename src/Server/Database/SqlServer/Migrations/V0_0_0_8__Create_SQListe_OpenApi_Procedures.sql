SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE  [sqliste].[p_openapi_convert_type_sql_to_openapi]
    @sql_type NVARCHAR(50),
    @select BIT = 0,
    @open_api_type NVARCHAR(50) = NULL OUT,
    @open_api_format NVARCHAR(50) = NULL OUT
AS
BEGIN
    IF @sql_type = 'int'
    BEGIN
        SET @open_api_type = 'integer'
        SET @open_api_format = 'int32'
    END
    ELSE IF @sql_type = 'bigint'
    BEGIN
        SET @open_api_type = 'integer'
        SET @open_api_format = 'int64'
    END
    ELSE IF @sql_type = 'smallint'
    BEGIN
        SET @open_api_type = 'integer'
        SET @open_api_format = 'int16'
    END
    ELSE IF @sql_type = 'tinyint'
    BEGIN
        SET @open_api_type = 'integer'
        SET @open_api_format = 'int8'
    END
    ELSE IF @sql_type = 'bit'
    BEGIN
        SET @open_api_type = 'boolean'
        SET @open_api_format = NULL
    END
    ELSE IF @sql_type IN ('decimal', 'numeric', 'money', 'smallmoney')
    BEGIN
        SET @open_api_type = 'number'
        SET @open_api_format = 'double'
    END
    ELSE IF @sql_type IN ('float', 'real')
    BEGIN
        SET @open_api_type = 'number'
        SET @open_api_format = 'float'
    END
    ELSE IF @sql_type IN ('date', 'datetime', 'datetime2', 'smalldatetime')
    BEGIN
        SET @open_api_type = 'string'
        SET @open_api_format = 'date-time'
    END
    ELSE IF @sql_type IN ('time')
    BEGIN
        SET @open_api_type = 'string'
        SET @open_api_format = 'time'
    END
    ELSE IF @sql_type IN ('char', 'varchar', 'text', 'nchar', 'nvarchar', 'ntext')
    BEGIN
        SET @open_api_type = 'string'
        SET @open_api_format = NULL
    END
    ELSE IF @sql_type IN ('binary', 'varbinary', 'image')
    BEGIN
        SET @open_api_type = 'string'
        SET @open_api_format = 'byte'
    END
    ELSE IF @sql_type IN ('uniqueidentifier')
    BEGIN
        SET @open_api_type = 'string'
        SET @open_api_format = NULL
    END
    ELSE IF (@sql_type IN ('timestamp'))
    BEGIN 
        SET @open_api_type = 'string' 
        SET @open_api_format = 'date-time' 
    END 
    ELSE 
    BEGIN 
        SET @open_api_type = @sql_type
        SET @open_api_format = NULL 
    END 

    IF (@select = 0)
        RETURN;

    SELECT 
        @open_api_type AS [type], 
        @open_api_format AS [format] 
END 
GO

CREATE OR ALTER    PROCEDURE [sqliste].[p_openapi_schema_json_create_components_def]
    @json NVARCHAR(MAX) OUT
AS 
BEGIN
    SET @json = '{ "schemas": {} }';
END
GO

CREATE OR ALTER    PROCEDURE [sqliste].[p_openapi_schema_json_add_type_def]
    @type_def_json NVARCHAR(MAX) OUT,
    @json NVARCHAR(MAX) OUT
AS 
BEGIN
    DECLARE 
        @name NVARCHAR(MAX),
        @path NVARCHAR(MAX)
    ;

    SET @name = JSON_VALUE(@type_def_json, '$.name');
    SET @type_def_json = JSON_MODIFY(@type_def_json, '$.name', NULL);

    SET @path = '$.schemas.' + @name;
    SET @json = JSON_MODIFY(@json, @path, JSON_QUERY(@type_def_json));
END
GO

CREATE OR ALTER    PROCEDURE [sqliste].[p_openapi_schema_json_create_type_def]
    @name NVARCHAR(MAX),
    @title NVARCHAR(MAX) = NULL,
    @description NVARCHAR(MAX) = NULL,
    @json NVARCHAR(MAX) OUT
AS 
BEGIN
    SET @json = '{ "required": [], "properties": {} }';

    SET @json = JSON_MODIFY(@json, '$.name', @name);

    IF (@title IS NOT NULL)
        SET @json = JSON_MODIFY(@json, '$.title', @title);

    IF (@description IS NOT NULL)
        SET @json = JSON_MODIFY(@json, '$.description', @description);
END
GO

CREATE OR ALTER    PROCEDURE [sqliste].[p_openapi_schema_json_create_property_def]
    @name NVARCHAR(MAX),
    @type NVARCHAR(MAX),
    @required BIT = 0,
    @min_length INT = NULL,
    @property_def_json NVARCHAR(MAX) OUT
AS 
BEGIN
    SET @property_def_json = '{}';

    SET @property_def_json = JSON_MODIFY(@property_def_json, '$.name', @name);
    SET @property_def_json = JSON_MODIFY(@property_def_json, '$._required', CAST(@required AS BIT));
    SET @property_def_json = JSON_MODIFY(
        @property_def_json, 
        '$.nullable', 
        CAST(
            CASE 
                WHEN @required = 0 
                THEN 1 
                ELSE 0 
            END 
            AS BIT
        )
    );

    -- IF @required = 1
    --     SET @property_def_json = JSON_MODIFY(@property_def_json, '$.Required', CAST(1 AS BIT));
    -- ELSE 
    --     SET @property_def_json = JSON_MODIFY(@property_def_json, '$.Nullable', CAST(1 AS BIT));

    DECLARE 
        @open_api_type NVARCHAR(50),
        @open_api_format NVARCHAR(50)
    ;

    EXEC [sqliste].[p_openapi_convert_type_sql_to_openapi] @sql_type = @type, @open_api_type = @open_api_type OUT, @open_api_format = @open_api_format OUT;

    SET @property_def_json = JSON_MODIFY(@property_def_json, '$.type', @open_api_type);

    IF (@min_length IS NOT NULL)
        SET @property_def_json = JSON_MODIFY(@property_def_json, '$.minLength', @min_length);

    IF (@open_api_format IS NOT NULL)
        SET @property_def_json = JSON_MODIFY(@property_def_json, '$.format', @open_api_format);
END
GO

CREATE OR ALTER    PROCEDURE [sqliste].[p_openapi_schema_json_add_property_def]
    @property_def_json NVARCHAR(MAX),
    @type_def_json NVARCHAR(MAX) OUT
AS 
BEGIN
    DECLARE 
        @name NVARCHAR(MAX),
        @property_path NVARCHAR(MAX),
        @is_required BIT
    ;

    SET @name = JSON_VALUE(@property_def_json, '$.name');
    SET @property_def_json = JSON_MODIFY(@property_def_json, '$.name', NULL);
    SET @property_path = '$.properties.' + @name;

    SET @is_required = JSON_VALUE(@property_def_json, '$._required');
    SET @property_def_json = JSON_MODIFY(@property_def_json, '$._required', NULL);

    IF (@is_required = 1)
        SET @type_def_json = JSON_MODIFY(@type_def_json, 'append $.required', @name)

    SET @type_def_json = JSON_MODIFY(@type_def_json, @property_path, JSON_QUERY(@property_def_json));
END
GO

CREATE OR ALTER    PROCEDURE [sqliste].[p_openapi_schema_json_create_property_ref]
    @name NVARCHAR(MAX),
    @type_name NVARCHAR(MAX),
    @property_def_json NVARCHAR(MAX) OUT
AS 
BEGIN
    DECLARE @reference_json NVARCHAR(MAX) = '{}';
    SET @property_def_json = '{}';

    SET @property_def_json = JSON_MODIFY(@property_def_json, '$.name', @name);
    -- SET @reference_json = JSON_MODIFY(@reference_json, '$.id', @type_name);
    -- SET @reference_json = JSON_MODIFY(@reference_json, '$.type', 0);
    
    -- SET @property_def_json = JSON_MODIFY(@property_def_json, '$.reference', JSON_QUERY(@reference_json));

    IF @type_name NOT LIKE '#/components/schemas/%'
        SET @type_name = '#/components/schemas/' + @type_name;

    SET @property_def_json = '{ "$ref": "' + @type_name + '" }'
END
GO

CREATE OR ALTER    PROCEDURE [sqliste].[p_openapi_schema_json_create_array_property_def]
    @name NVARCHAR(MAX),
    @items_type_def_json NVARCHAR(MAX),
    @property_def_json NVARCHAR(MAX) OUT
AS 
BEGIN
    SET @property_def_json = '{ "type": "array" }';

    SET @property_def_json = JSON_MODIFY(@property_def_json, '$.name', @name);
    SET @property_def_json = JSON_MODIFY(@property_def_json, '$.items', JSON_QUERY(@items_type_def_json));
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
