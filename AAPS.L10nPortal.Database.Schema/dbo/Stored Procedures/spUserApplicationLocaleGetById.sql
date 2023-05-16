CREATE PROCEDURE [dbo].[spUserApplicationLocaleGetById]
	@userId					UNIQUEIDENTIFIER,
	@applicationLocaleId	INT
AS
BEGIN
	SET NOCOUNT ON;

	EXEC spUserApplicationLocaleCheckPermissions @userId = @userId, @applicationLocaleId = @applicationLocaleId
	
	EXEC [spUserApplicationLocaleGetByIdInternal] @userId = @userId, @applicationLocaleId = @applicationLocaleId
END