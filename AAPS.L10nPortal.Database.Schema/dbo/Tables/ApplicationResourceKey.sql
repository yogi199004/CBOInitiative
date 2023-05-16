CREATE TABLE [dbo].[ApplicationResourceKey] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationId] INT            NOT NULL,
    [Name]          NVARCHAR (256) NOT NULL,
    [TypeId]        INT            NOT NULL,
    [Description]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ApplicationResourceKey] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ApplicationResourceKey_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Application] ([Id]),
    CONSTRAINT [FK_ApplicationResourceKey_ApplicationResourceKeyType] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[ApplicationResourceKeyType] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_ApplicationResourceKey]
    ON [dbo].[ApplicationResourceKey]([ApplicationId] ASC, [Name] ASC);

