CREATE PROCEDURE [dbo].[spUserApplicationLocaleCheckPermissions]
	@userId					UNIQUEIDENTIFIER,
	@applicationLocaleId	INT
AS
BEGIN
	IF NOT EXISTS(
		SELECT ual.UserId FROM UserApplicationLocale AS ual WITH (NOLOCK)
			WHERE ual.UserId = @userId AND ual.ApplicationLocaleId = @applicationLocaleId
	)
	BEGIN
		EXEC [throwPermissionException]
	END
END