CREATE TABLE [dbo].[Language] (
    [Id]              INT           NOT NULL,
    [Code]            NVARCHAR (50) NOT NULL,
    [DefaultLocaleId] INT           NULL,
    CONSTRAINT [PK_Language_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Language_Locale] FOREIGN KEY ([DefaultLocaleId]) REFERENCES [dbo].[Locale] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Language]
    ON [dbo].[Language]([Code] ASC);

