CREATE TYPE [dbo].[ResourceKeyValue] AS TABLE (
    [Key]         NVARCHAR (256) NOT NULL,
    [Value]       NVARCHAR (MAX) NULL,
    [TypeId]      INT            NOT NULL,
    [Description] NVARCHAR (MAX) NULL);

