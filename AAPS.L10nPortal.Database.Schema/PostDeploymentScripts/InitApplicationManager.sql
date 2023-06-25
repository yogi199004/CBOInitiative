IF NOT EXISTS(SELECT 1 FROM [Application] WHERE Id = $(applicationId))
BEGIN
	PRINT 'Application not found'
	RETURN
END
--@userUId and @enUsApplicationLocaleId Variables renamed to ensure its not failing in DacPac
DECLARE @userUIdGE UNIQUEIDENTIFIER

SELECT @userUIdGE = Id 
	FROM [User]
WHERE 
	Email = '$(managerEmail)' 

IF(@userUIdGE IS NULL)
BEGIN 
	PRINT 'User ''$(managerEmail)'' not found'
	INSERT INTO [dbo].[User] ([Id], [Email], [CreatedDate])
		VALUES (NEWID(), '$(managerEmail)', GETUTCDATE())

		SELECT @userUIdGE = Id FROM [User] WHERE  
	     Email = '$(managerEmail)' 


END

	

	-- en-US locale for application
	PRINT 'Check en-US locale for application'
	IF NOT EXISTS(SELECT 1 FROM [ApplicationLocale] WHERE ApplicationId = $(applicationId) AND LocaleId = $(enUsLocaleId))
	BEGIN
		PRINT 'Create en-US locale for application'
		INSERT INTO [dbo].[ApplicationLocale] ([ApplicationId],[LocaleId],[Active],[CreatedDate],[CreatedBy])
		VALUES ($(applicationId), $(enUsLocaleId), 1, GETUTCDATE(), @userUIdGE)
	END

	DECLARE @enUsApplicationLocaleIdL10n INT

	PRINT 'Get en-US application locale id'
	SELECT @enUsApplicationLocaleIdL10n = Id
		FROM [ApplicationLocale]
	WHERE ApplicationId = $(applicationId) AND LocaleId = $(enUsLocaleId)

	-- en-US assignment, if missing for any user
	PRINT 'Check en-US application locale id assignment'
	if not exists (select 1 from UserApplicationLocale where ApplicationLocaleId = @enUsApplicationLocaleIdL10n )
	BEGIN
		PRINT 'Create en-US application locale id assignment' 
		Insert Into [dbo].[UserApplicationLocale]
		(
		UserId
		,ApplicationLocaleId
		)
		values 
		(@userUIdGE,@enUsApplicationLocaleIdL10n)
		

		END
		

