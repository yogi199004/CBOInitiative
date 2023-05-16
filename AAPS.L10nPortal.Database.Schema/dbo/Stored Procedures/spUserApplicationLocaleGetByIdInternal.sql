CREATE PROCEDURE [dbo].[spUserApplicationLocaleGetByIdInternal]
	@userId					UNIQUEIDENTIFIER,
	@applicationLocaleId	INT
AS
BEGIN
	SET NOCOUNT ON;
	Declare @appManagerCount int

	select @appManagerCount = [dbo].[fn_GetAppManagerCount] (
   @applicationLocaleId)

	SELECT 
		 [ApplicationLocaleId]
		,[UserId]
		,[UserEmail]
		,[PreferredName]
		,[ApplicationId]
		,[ApplicationName]
		,[LocaleId]
		,[LocaleCode]
		,[NativeName]
		,[EnglishName]
		,[NativeLanguageName]
		,[EnglishLanguageName]
		,[NativeCountryName]
		,[EnglishCountryName]
		,[UpdatedDate]
		,[UpdatedBy]
		,[TotalLabelsCount]
		,[TotalAssetsCount]
		,[UploadedAssetCount]
		,CAST(CASE WHEN [UserId] = @userId THEN 1 ELSE 0 END AS bit) AS [CanEdit]
		,@appManagerCount as AppManagerCount
	FROM 
		[dbo].[UserApplicationLocaleView] WITH (NOLOCK)
	WHERE
		[ApplicationLocaleId] = @applicationLocaleId
END