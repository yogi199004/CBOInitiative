CREATE TABLE [dbo].[AuditApplicationResourceKey]
(
	[Id] INT  IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[AppId] INT NOT NULL,
	[DeletedDate] DATETIME NOT NULL
)
