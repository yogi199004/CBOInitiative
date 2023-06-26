

Create PROCEDURE [dbo].[spOnboardApplication]
	@UserEmail				varchar(100),
	@assignToUserEmail		varchar(100),
	@ApplicationName	nvarchar(max)
AS
BEGIN
DECLARE @maxAppId int
DECLARE @appId int
DECLARE @Repoindex int
DECLARE @userId uniqueIdentifier
DECLARE @assignToUserId uniqueIdentifier

DECLARE @applicationLocaleId INT
DECLARE @newApplicationLocaleIds TABLE (ApplicationLocaleId INT)
DECLARE @IsAdmin INT

select @userId = [id] from [User] where email = @UserEmail

EXEC @IsAdmin = [dbo].[spCheckAdminPermissions] @userId = @userId
IF NOT (@IsAdmin =0)
	BEGIN
		EXEC [throwPermissionException]
	END

IF EXISTS(SELECT Id FROM [Application] WITH (NOLOCK) WHERE [Name] = @ApplicationName)
	BEGIN
	EXEC [throwApplicationAlreadyOnboardedException]
	END
ELSE
--------- OnBoarded App 
	BEGIN

	select @maxAppId = id from [Application]
	INSERT INTO [dbo].[Application] (Name, RedisInstance, RepoIndex)
	VALUES(@ApplicationName, Null, Null)
	select @appId=id from Application where Name = @ApplicationName
	END

---------Create EN-US locale for onboarded App 
	BEGIN
	PRINT 'Create en-US locale for application'
	INSERT INTO [dbo].[ApplicationLocale]
	([ApplicationId]
	,[LocaleId]
	,[Active]
	,[CreatedDate]
	,[CreatedBy])
	OUTPUT INSERTED.Id INTO @newApplicationLocaleIds
	VALUES
	(@appId
	,91
	,1
	,GETUTCDATE()
	,@userId) 
	END

-- en-US assignment, if missing for any user
select @assignToUserId = [id] from [User] where email = @assignToUserEmail


	BEGIN
	INSERT INTO [UserApplicationLocale]
	SELECT @assignToUserId, ApplicationLocaleId
	FROM @newApplicationLocaleIds
	END

END
GO


