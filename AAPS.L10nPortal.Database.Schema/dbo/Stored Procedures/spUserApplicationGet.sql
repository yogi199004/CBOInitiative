CREATE PROCEDURE [dbo].[spUserApplicationGet]
	@userId    UNIQUEIDENTIFIER,
	@canManage BIT = NULL
AS
	SELECT 
		 [ApplicationId]   AS [Id]
		,[ApplicationName] AS [Name]
		--,[CanManage]       AS [CanManage]
	FROM [UserApplicationView] WITH (NOLOCK)
	WHERE 
		UserId = @userId
		AND (@canManage IS NULL OR (CanManage = @canManage))