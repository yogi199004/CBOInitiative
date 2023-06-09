CREATE PROCEDURE [dbo].[spAddAppManager]
	@userId					UNIQUEIDENTIFIER,
	@applicationLocaleId	INT,
	@assignToUserId			UNIQUEIDENTIFIER
	
AS
BEGIN
	DECLARE @applicationId INT
	DECLARE @appManagerCount  INT

	SELECT @applicationId = ApplicationId FROM [ApplicationLocale] WITH (NOLOCK) WHERE Id = @applicationLocaleId

	IF(@applicationId IS NULL)
	BEGIN
		EXEC [throwPermissionException]
	END

	EXEC [dbo].[spUserApplicationManagerCheckPermissions] @userId = @userId, @applicationId = @applicationId

	IF NOT EXISTS(SELECT Id FROM [Application] WHERE Id =  @applicationId)
	BEGIN
		EXEC [throwApplicationNotFoundException]
	END
    
  SELECT @appManagerCount = [dbo].[fn_GetAppManagerCount] (@applicationLocaleId)
  
   IF(@appManagerCount>=2)
   BEGIN
       EXEC [throwDualAppManagerException]
   END
	
	IF EXISTS(
		SELECT ual.UserId FROM [UserApplicationLocale]  AS ual WITH (NOLOCK)
		INNER JOIN [ApplicationLocale] AS al WITH (NOLOCK) ON al.Id = ual.ApplicationLocaleId
		WHERE al.Id = @applicationLocaleId AND ual.UserId = @assignToUserId
	)
	BEGIN
		EXEC [throwApplicationLocaleAlreadyAssignedException]
	END

	

	INSERT INTO [UserApplicationLocale] (UserId,ApplicationLocaleId) VALUES(@assignToUserId,@applicationLocaleId)

END



