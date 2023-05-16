IF NOT EXISTS(SELECT 1 FROM [Application] WHERE Id = $(applicationId))
BEGIN
	PRINT 'Application not found'
	RETURN
END
--@userUId and @enUsApplicationLocaleId Variables renamed to ensure its not failing in DacPac
DECLARE @userUIdGE UNIQUEIDENTIFIER

SELECT @userUIdGE = GlobalPersonUId 
	FROM OPM_Cache.dbo.GlobalEmployee 
WHERE 
	Email = '$(managerEmail)' AND
	EmployeeStatusCode = 1 -- Active

IF(@userUIdGE IS NULL)
BEGIN 
	PRINT 'User ''$(managerEmail)'' not found'
END
ELSE
BEGIN
	-- User
	PRINT 'Check user'
	IF NOT EXISTS(SELECT 1 FROM [User] WHERE Id = @userUIdGE)
	BEGIN
		PRINT 'Create user'
		INSERT INTO [dbo].[User] ([Id], [Email], [CreatedDate])
		VALUES (@userUIdGE, '$(managerEmail)', GETUTCDATE())
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
	IF NOT EXISTS(SELECT 1 FROM [UserApplicationLocale] WHERE ApplicationLocaleId = @enUsApplicationLocaleIdL10n)
	BEGIN
		PRINT 'Create en-US application locale id assignment'
		INSERT INTO [dbo].[UserApplicationLocale]([UserId], [ApplicationLocaleId])
		VALUES (@userUIdGE, @enUsApplicationLocaleIdL10n)
	END
END