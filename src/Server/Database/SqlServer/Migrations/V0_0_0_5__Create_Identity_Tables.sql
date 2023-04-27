SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sys_identity_permissions] (
    [id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [name]       NVARCHAR (500) NOT NULL,
    [created_at] DATETIME       CONSTRAINT [DEFAULT_sys_identity_permissions_created_at] DEFAULT (getdate()) NOT NULL,
    [updated_at] DATETIME       CONSTRAINT [DEFAULT_sys_identity_permissions_updated_at] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_sys_identity_permissions] PRIMARY KEY CLUSTERED ([id] ASC)
);
GO

CREATE TABLE [dbo].[sys_identity_roles] (
    [id]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [name]       NVARCHAR (80) NOT NULL,
    [created_at] DATETIME      CONSTRAINT [DEFAULT_sys_identity_roles_created_at] DEFAULT (getdate()) NOT NULL,
    [updated_at] DATETIME      CONSTRAINT [DEFAULT_sys_identity_roles_updated_at] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_sys_identity_roles] PRIMARY KEY CLUSTERED ([id] ASC)
);
GO

CREATE TABLE [dbo].[sys_identity_users] (
    [id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [first_name] NVARCHAR (80)  NOT NULL,
    [last_name]  NVARCHAR (80)  NOT NULL,
    [password]   VARBINARY (64) NOT NULL,
    [email]      NVARCHAR (500) NOT NULL,
    [created_at] DATETIME       CONSTRAINT [DEFAULT_sys_identity_users_created_at] DEFAULT (getdate()) NOT NULL,
    [updated_at] DATETIME       CONSTRAINT [DEFAULT_sys_identity_users_updated_at] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_NewTable] PRIMARY KEY CLUSTERED ([id] ASC)
);
GO

CREATE TABLE [dbo].[sys_identity_user_role_links] (
    [user_id]    BIGINT   NOT NULL,
    [role_id]    BIGINT   NOT NULL,
    [created_at] DATETIME CONSTRAINT [DEFAULT_sys_identity_user_role_links_created_at] DEFAULT (getdate()) NOT NULL,
    [updated_at] DATETIME CONSTRAINT [DEFAULT_sys_identity_user_role_links_updated_at] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_sys_identity_user_role_links] PRIMARY KEY CLUSTERED ([user_id] ASC, [role_id] ASC),
    CONSTRAINT [FK_sys_identity_user_role_links_sys_identity_roles] FOREIGN KEY ([role_id]) REFERENCES [dbo].[sys_identity_roles] ([id]),
    CONSTRAINT [FK_sys_identity_user_role_links_sys_identity_users] FOREIGN KEY ([user_id]) REFERENCES [dbo].[sys_identity_users] ([id])
);
GO

CREATE TABLE [dbo].[sys_identity_role_permission_link] (
    [role_id]       BIGINT   NOT NULL,
    [permission_id] BIGINT   NOT NULL,
    [created_at]    DATETIME CONSTRAINT [DEFAULT_sys_identity_role_permission_link_created_at] DEFAULT (getdate()) NOT NULL,
    [updated_at]    DATETIME CONSTRAINT [DEFAULT_sys_identity_role_permission_link_updated_at] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_sys_identity_role_permission_link] PRIMARY KEY CLUSTERED ([role_id] ASC, [permission_id] ASC),
    CONSTRAINT [FK_sys_identity_role_permission_link_sys_identity_permissions] FOREIGN KEY ([permission_id]) REFERENCES [dbo].[sys_identity_permissions] ([id]),
    CONSTRAINT [FK_sys_identity_role_permission_link_sys_identity_roles] FOREIGN KEY ([role_id]) REFERENCES [dbo].[sys_identity_roles] ([id])
);
GO
