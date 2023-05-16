CREATE TABLE [dbo].[User] (
    [Id]			UNIQUEIDENTIFIER	NOT NULL,
    [Email]			NVARCHAR (254)		NOT NULL,
    [PreferredName] NVARCHAR (254)		NULL,
    [CreatedBy]		UNIQUEIDENTIFIER	NULL,
    [CreatedDate]	DATETIME			NULL,
    [UpdatedBy]		UNIQUEIDENTIFIER	NULL,
    [UpdatedDate]	INT					NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
);