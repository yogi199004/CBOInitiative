CREATE VIEW [dbo].[ApplicationLocaleView]
AS
SELECT
	 al.Id                      AS [ApplicationLocaleId]
	,a.Id                       AS [ApplicationId]
	,a.[Name]                   AS [ApplicationName]
	,al.LocaleId                AS [LocaleId]
	,l.Code                     AS [LocaleCode]
    ,l.NativeName               AS [NativeName]
	,l.EnglishName              AS [EnglishName]
	,l.NativeLanguageName		AS [NativeLanguageName]
	,l.EnglishLanguageName		AS [EnglishLanguageName]
	,l.NativeCountryName		AS [NativeCountryName]
	,l.EnglishCountryName		AS [EnglishCountryName]
	,al.UpdatedDate             AS [UpdatedDate]
	,al.UpdatedBy				AS [UpdatedBy]
    ,isnull(akvc.[TotalLabelsCount], 0)  AS [TotalLabelsCount]
    ,isnull(akvc.[TotalAssetsCount], 0)  AS [TotalAssetsCount]
	,isnull(acv.[UploadedAssetCount], 0)  AS [UploadedAssetCount]
	,a.RedisInstance
	, a.RepoIndex
	FROM [ApplicationLocale]    AS al WITH (NOLOCK)
	INNER JOIN [Locale]         AS l WITH (NOLOCK) ON l.Id = al.localeid
	INNER JOIN [Application]    AS a WITH (NOLOCK) ON a.Id = al.applicationid
	Left JOIN [ApplicationAssetCountView]    AS acv WITH (NOLOCK) ON a.Id = acv.applicationid and l.Id = acv.LocaleId
    LEFT JOIN [ApplicationKeyValueCountView] AS akvc WITH (NOLOCK) ON akvc.ApplicationId = al.ApplicationId