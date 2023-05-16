CREATE TABLE [dbo].[Locale] (
    [Id]					INT				NOT NULL,
    [Code]					NVARCHAR (50)	NOT NULL,
	[NativeName]			NVARCHAR (128)	NOT NULL DEFAULT '',
	[EnglishName]			NVARCHAR (128)	NOT NULL DEFAULT '',
	[NativeLanguageName]	NVARCHAR (128)	NOT NULL DEFAULT '',
	[EnglishLanguageName]	NVARCHAR (128)	NOT NULL DEFAULT '',
	[NativeCountryName]		NVARCHAR (128)	NOT NULL DEFAULT '',
	[EnglishCountryName]	NVARCHAR (128)	NOT NULL DEFAULT '',

    CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_Language_Code]
    ON [dbo].[Locale]([Code] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Locale]
    ON [dbo].[Locale]([Code] ASC);

