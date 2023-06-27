CREATE PROCEDURE [dbo].[spUserApplicationLocaleGetById]
	@userEmailId					varchar(100),
	@applicationLocaleId	INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @userID uniqueIdentifier 
	SELECT @userID = ID from [User] where email = @userEmailId
	EXEC spUserApplicationLocaleCheckPermissions @userId = @userId, @applicationLocaleId = @applicationLocaleId
	
	EXEC [spUserApplicationLocaleGetByIdInternal] @userId = @userId, @applicationLocaleId = @applicationLocaleId
END