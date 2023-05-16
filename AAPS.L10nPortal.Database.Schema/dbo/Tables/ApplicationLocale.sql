CREATE TABLE [dbo].[ApplicationLocale] (
    [Id]            INT      IDENTITY (1, 1) NOT NULL,
    [ApplicationId] INT      NOT NULL,
    [LocaleId]      INT      NOT NULL,
    [Active]        BIT      NOT NULL,
    [CreatedDate]   DATETIME NOT NULL,
    [CreatedBy]     UNIQUEIDENTIFIER      NOT NULL,
    [UpdatedDate]   DATETIME NULL,
    [UpdatedBy]     UNIQUEIDENTIFIER      NULL,
    CONSTRAINT [PK_ApplicationLocale] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ApplicationLocale_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Application] ([Id]),
    CONSTRAINT [FK_ApplicationLocale_Locale] FOREIGN KEY ([LocaleId]) REFERENCES [dbo].[Locale] ([Id])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_ApplicationLocale]
    ON [dbo].[ApplicationLocale]([ApplicationId] ASC, [LocaleId] ASC);

