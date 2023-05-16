CREATE PROCEDURE [dbo].[spCheckAppMgrPermissions]
	@userId					UNIQUEIDENTIFIER,
	@applicationId	INT
AS
BEGIN
	IF EXISTS(select ual.UserId from Applicationlocale al WITH (NOLOCK) inner join UserApplicationLocale ual WITH (NOLOCK) 
		on al.id=ual.ApplicationLocaleId where al.LocaleId = 91 and al.ApplicationId = @applicationId 
		and ual.UserId = @userId)
	RETURN 0
	ELSE RETURN -1
END