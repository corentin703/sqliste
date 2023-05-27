SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [sqliste].[app_events] (
    [id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [type]        VARCHAR (250)  NOT NULL,
    [name]        VARCHAR (250)  NOT NULL,
    [args]        NVARCHAR (MAX) NULL,
    [inserted_at] DATETIME       CONSTRAINT [DEFAULT_app_events_inserted_at] DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] DESC)
    );
GO
