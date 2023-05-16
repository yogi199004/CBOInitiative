CREATE PROCEDURE [dbo].[spUserApplicationLocaleCreate]
	@userId				UNIQUEIDENTIFIER,
	@applicationId		INT,
	@localeId			INT,
	@assignToUserId		UNIQUEIDENTIFIER
AS
BEGIN
	DECLARE @IsAppMgr INT
	EXEC @IsAppMgr = [dbo].[spCheckAppMgrPermissions] @userId = @userId, @applicationId=@applicationId
	IF NOT (@IsAppMgr =0)
	BEGIN
		EXEC [throwPermissionException]
	END

	IF NOT EXISTS(SELECT Id FROM [Application] WHERE Id =  @applicationId)
	BEGIN
		EXEC [throwApplicationNotFoundException]
	END

	IF NOT EXISTS(SELECT Id FROM [Locale] WHERE Id =  @localeId)
	BEGIN
		EXEC [throwLocaleNotFoundException]
	END
	
	IF EXISTS(
		SELECT ual.UserId FROM [ApplicationLocale] AS al WITH (NOLOCK)
		INNER JOIN [UserApplicationLocale] AS ual ON ual.ApplicationLocaleId = al.Id
		WHERE al.ApplicationId = @applicationId AND al.LocaleId = @localeId
	)
	BEGIN
		EXEC [throwApplicationLocaleAlreadyAssignedException]
	END

	DECLARE @existingApplicationLocaleId INT
	DECLARE @newApplicationLocaleIds TABLE (ApplicationLocaleId INT)

	SELECT @existingApplicationLocaleId = Id FROM [dbo].[ApplicationLocale] WITH (NOLOCK)
	WHERE ApplicationId = @applicationId AND LocaleId = @localeId

	IF (@existingApplicationLocaleId IS NULL)
	BEGIN
		INSERT INTO [dbo].[ApplicationLocale]
			([ApplicationId]
			,[LocaleId]
			,[Active]
			,[CreatedDate]
			,[CreatedBy])
		OUTPUT INSERTED.Id INTO @newApplicationLocaleIds
		VALUES
			(@applicationId
			,@localeId
			,1
			,GETUTCDATE()
			,@userId)
	END
	ELSE
	BEGIN
		INSERT @newApplicationLocaleIds VALUES (@existingApplicationLocaleId)
	END


	INSERT INTO [UserApplicationLocale] 
	SELECT @assignToUserId, ApplicationLocaleId
		FROM @newApplicationLocaleIds 

	DECLARE @newApplicationLocaleId INT

	-- Should be one element
	SELECT @newApplicationLocaleId = ApplicationLocaleId FROM @newApplicationLocaleIds

	-- return created UserApplicationLocale
	EXEC [spUserApplicationLocaleGetByIdInternal] @userId = @userId, @ApplicationLocaleId = @newApplicationLocaleId
END