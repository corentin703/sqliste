SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[p_identity_user_login]
     @email NVARCHAR(500)
    ,@password NVARCHAR(MAX)
    ,@jwt NVARCHAR(MAX) OUT
AS 
BEGIN
    DECLARE 
        @user_id BIGINT
    ;
    
    SELECT @user_id = [id]
    FROM [sys_identity_users]
    WHERE [email] = @email AND [password] = [dbo].[f_hash_password](@password);

    IF (@user_id IS NULL)
    BEGIN
        RAISERROR('Login failed', 18, 1);
    END

    DECLARE @user_roles TABLE (
        [id] BIGINT,
        [name] NVARCHAR(80)
    );

    DECLARE @user_permissions TABLE (
        [id] BIGINT,
        [name] NVARCHAR(500)
    );

    DECLARE @json_payload NVARCHAR(MAX);
    DECLARE @jwt_secret NVARCHAR(MAX);

    SELECT @jwt_secret = [value] FROM [sys_settings]
    WHERE [key] = 'jwt_secret';

    INSERT INTO @user_roles
    SELECT [id], [name]
    FROM [sys_identity_roles]
    WHERE [id] IN (
        SELECT [role_id] FROM [sys_identity_user_role_links]
        WHERE [user_id] = @user_id
    );

    INSERT INTO @user_permissions
    SELECT DISTINCT [id], [name]
    FROM [sys_identity_permissions]
    INNER JOIN [sys_identity_role_permission_link] ON [sys_identity_role_permission_link].[permission_id] = [sys_identity_permissions].[id]
    WHERE [sys_identity_role_permission_link].[role_id] IN (
        SELECT [id] FROM @user_roles
    );

    SET @json_payload = (
        SELECT
            @user_id AS "id"
            ,@email AS "email" 
            ,(
                SELECT * FROM @user_roles
                ORDER BY [id]    
                FOR JSON PATH
            ) AS "roles"
            ,(
                SELECT * FROM @user_permissions 
                ORDER BY [id] 
                FOR JSON PATH
            ) AS "permissions"
        FOR JSON PATH
    );

    SELECT @jwt = [dbo].[f_jwt_encode](
        '{"alg":"HS256","typ":"JWT"}'
        ,@json_payload
        ,@jwt_secret
    );
END
GO