CREATE PROCEDURE [dbo].[spApplicationLocaleGetList]
AS

SELECT
	 [ApplicationLocaleId]
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
	,[RedisInstance]
	,[RepoIndex]
FROM [ApplicationLocaleView] WITH (NOLOCK)
