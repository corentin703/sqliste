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

EXEC [sqliste].[p_openapi_document_get]