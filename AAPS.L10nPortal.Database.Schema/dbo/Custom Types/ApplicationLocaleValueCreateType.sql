create type dbo.ApplicationLocaleValueCreateType as table
(
	[Key] nvarchar(255),
	[TranslatedValue] nvarchar(max)
)