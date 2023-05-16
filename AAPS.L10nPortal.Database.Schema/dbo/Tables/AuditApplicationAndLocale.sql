CREATE TABLE [dbo].[AuditApplicationAndLocale]
(
	[Id] INT  IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[AppId] INT NOT NULL,
	[LocaleId] INT NOT NULL,
	[LocaleDeleted] NVARCHAR  (128) NOT NULL,
	[DeletedDate] DATETIME NOT NULL
)
