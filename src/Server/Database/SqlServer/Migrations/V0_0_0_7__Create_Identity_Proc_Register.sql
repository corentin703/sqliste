SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[p_identity_user_register]
     @first_name NVARCHAR(80)
    ,@last_name NVARCHAR(80)
    ,@email NVARCHAR(500)
    ,@password NVARCHAR(MAX)
AS 
BEGIN
    
    IF EXISTS (SELECT 1 FROM [sys_identity_users] WHERE [email] = @email)
    BEGIN
        RAISERROR('user is already registered', 18, 0);
    END

    INSERT INTO [sys_identity_users] (
         [first_name]
        ,[last_name]
        ,[password]
        ,[email]
    ) VALUES (
         @first_name
        ,@last_name
        ,[dbo].[f_hash_password](@password)
        ,@email
    );
END
GO
