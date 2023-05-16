CREATE PROCEDURE [dbo].[spUserApplicationLocaleReassign]
	@userId					UNIQUEIDENTIFIER,
	@applicationLocaleId	INT,
	@assignFromUserId		UNIQUEIDENTIFIER,
	@assignToUserId			UNIQUEIDENTIFIER
AS
BEGIN
	DECLARE @applicationId INT
	DECLARE @LocaleId INT
	DECLARE @IsAdmin INT
	DECLARE @IsAppMgr INT

	SELECT @applicationId = ApplicationId,@LocaleId = LocaleId  FROM [ApplicationLocale] WHERE Id = @applicationLocaleId

	IF(@applicationId IS NULL)
	BEGIN
		EXEC [throwPermissionException]
	END

	EXEC @IsAdmin = [dbo].[spCheckAdminPermissions] @userId = @userId
	EXEC @IsAppMgr = [dbo].[spCheckAppMgrPermissions] @userId = @userId, @applicationId=@applicationId

	IF(@LocaleId = 91)
	BEGIN		
		IF NOT (@IsAdmin = 0 OR @IsAppMgr =0)
		BEGIN
			EXEC [throwPermissionException]
		END	
	END
	ELSE
	BEGIN
		IF NOT (@IsAppMgr =0)
		BEGIN
			EXEC [throwPermissionException]
		END	
	END

	IF NOT EXISTS(SELECT Id FROM [Application] WHERE Id =  @applicationId)
	BEGIN
		EXEC [throwApplicationNotFoundException]
	END

	IF EXISTS(
		SELECT ual.UserId FROM [UserApplicationLocale] AS ual WITH (NOLOCK)
		INNER JOIN [ApplicationLocale] AS al WITH (NOLOCK) ON al.Id = ual.ApplicationLocaleId
		WHERE al.Id = @applicationLocaleId AND ual.UserId = @assignToUserId
	)
	BEGIN
		EXEC [throwApplicationLocaleAlreadyAssignedException]
	END

	UPDATE [UserApplicationLocale] 
	SET UserId = @assignToUserId
	WHERE UserId = @assignFromUserId AND ApplicationLocaleId = @applicationLocaleId

	-- return updated UserApplicationLocale
	EXEC [spUserApplicationLocaleGetByIdInternal] @userId = @userId, @applicationLocaleId = @applicationLocaleId
END