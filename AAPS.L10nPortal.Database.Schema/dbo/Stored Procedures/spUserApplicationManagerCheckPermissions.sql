CREATE PROCEDURE [dbo].[spUserApplicationManagerCheckPermissions]
	@userId			UNIQUEIDENTIFIER,
	@applicationId	INT
AS
BEGIN
    	
    IF NOT EXISTS(SELECT * FROM [UserRolemapping] ur WITH (NOLOCK) INNER JOIN RoleMaster r WITH (NOLOCK) on ur.roleId=r.Id WHERE ur.GUId=@userId and r.Id=1 and ur.Active=1 )
	BEGIN
	
	IF NOT EXISTS(
		SELECT ual.UserId FROM UserApplicationLocale AS ual WITH (NOLOCK)
		INNER JOIN ApplicationLocale AS al WITH (NOLOCK) ON al.Id = ual.ApplicationLocaleId
		INNER JOIN Locale AS l WITH (NOLOCK) ON l.Id = al.LocaleId
		WHERE ual.UserId = @userId AND l.Code = 'en-US'
	)
	BEGIN
		EXEC [throwPermissionException]
	END
	END
END