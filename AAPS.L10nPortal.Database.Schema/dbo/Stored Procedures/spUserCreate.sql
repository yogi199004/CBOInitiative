CREATE PROCEDURE [dbo].[spUserCreate]
	@userId			UNIQUEIDENTIFIER,
	@newUserId		UNIQUEIDENTIFIER,
	@newUserEmail	NVARCHAR(254)
AS
	MERGE INTO dbo.[User] AS t
	USING (VALUES(@newUserId, @newUserEmail)
	) AS s ([Id], [Email])
	ON t.[Id] = s.[Id]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([Id], [Email], [CreatedBy], [CreatedDate])
		VALUES ([Id], [Email], @userId, GETUTCDATE());