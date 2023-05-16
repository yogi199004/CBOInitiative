CREATE TABLE [dbo].[ApplicationResourceValue] (
    [ApplicationResourceKeyId]	INT            NOT NULL,
    [LocaleId]					INT            NOT NULL,
    [Value]						NVARCHAR (MAX) NULL,
    [CreatedDate]				DATETIME NOT NULL,
    [CreatedBy]					UNIQUEIDENTIFIER      NOT NULL,
    [UpdatedDate]				DATETIME NULL,
    [UpdatedBy]					UNIQUEIDENTIFIER      NULL,
    CONSTRAINT [PK_ApplicationResourceValue] PRIMARY KEY CLUSTERED ([ApplicationResourceKeyId] ASC, [LocaleId] ASC),
    CONSTRAINT [FK_ApplicationResourceValue_ApplicationResourceKey] FOREIGN KEY ([ApplicationResourceKeyId]) REFERENCES [dbo].[ApplicationResourceKey] ([Id]),
    CONSTRAINT [FK_ApplicationResourceValue_Locale] FOREIGN KEY ([LocaleId]) REFERENCES [dbo].[Locale] ([Id])
);

