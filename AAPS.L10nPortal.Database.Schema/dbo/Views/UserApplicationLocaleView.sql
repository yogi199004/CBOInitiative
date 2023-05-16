CREATE VIEW [dbo].[UserApplicationLocaleView]
AS 
SELECT
	 al.[ApplicationLocaleId]
	,ual.UserId             AS [UserId]
	,'u.Email'              AS [UserEmail]      -- OBSOLETE. TODO: Remove
	,'u.PreferredName'      AS [PreferredName]  -- OBSOLETE. TODO: Remove
	,al.[ApplicationId]
	,al.[ApplicationName]
	,al.[LocaleId]
	,al.[LocaleCode]
    ,al.[NativeName]
	,al.[EnglishName]
	,al.[NativeLanguageName]
	,al.[EnglishLanguageName]
	,al.[NativeCountryName]
	,al.[EnglishCountryName]
	,al.[UpdatedDate]
	,al.[UpdatedBy]
	,al.[TotalLabelsCount]
	,al.[TotalAssetsCount]
	,al.[UploadedAssetCount]
	FROM [ApplicationLocaleView] AS al WITH (NOLOCK)
	INNER JOIN [UserApplicationLocale] AS ual WITH (NOLOCK) ON ual.ApplicationLocaleId = al.ApplicationLocaleId
	INNER JOIN [User] AS u WITH (NOLOCK) on u.Id = ual.UserId