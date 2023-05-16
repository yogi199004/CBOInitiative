CREATE TABLE [dbo].[Application] (
    [Id]   INT        NOT NULL IDENTITY (1,1),
    [Name] NVARCHAR  (128) NULL,	
    [RedisInstance] [int] NULL,
	[RepoIndex] [int] NULL,
    CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED ([Id] ASC)
);

