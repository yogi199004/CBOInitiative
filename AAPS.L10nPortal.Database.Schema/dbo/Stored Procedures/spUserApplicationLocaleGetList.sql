CREATE PROCEDURE spUserApplicationLocaleGetList
	@userId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	Create table #tblUserApplicationLocales
(ApplicationLocaleId int
,UserId uniqueidentifier
,UserEmail varchar(7)
,PreferredName varchar(15)
,ApplicationId int
,ApplicationName nvarchar(128)
,LocaleId int
,LocaleCode nvarchar(50)
,NativeName nvarchar(128)
,EnglishName nvarchar(128)
,NativeLanguageName nvarchar(128)
,EnglishLanguageName nvarchar(128)
,NativeCountryName nvarchar(128)
,EnglishCountryName nvarchar(128)
,UpdatedDate datetime
,UpdatedBy uniqueidentifier
,TotalLabelsCount int
,TotalAssetsCount int
, [CanEdit] bit
,AppManagerCount int
,UploadedAssetCount int)

INSERT INTO #tblUserApplicationLocales 
(
ApplicationLocaleId 
,UserId 
,UserEmail 
,PreferredName 
,ApplicationId 
,ApplicationName 
,LocaleId
,LocaleCode 
,NativeName 
,EnglishName 
,NativeLanguageName 
,EnglishLanguageName 
,NativeCountryName 
,EnglishCountryName 
,UpdatedDate 
,UpdatedBy 
,TotalLabelsCount 
,TotalAssetsCount
,UploadedAssetCount
,[CanEdit]
)
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
FROM
[dbo].[UserApplicationLocaleView] WITH (NOLOCK)
--ORDER BY [CanEdit] desc,[ApplicationName], [LocaleCode]
--END

Create TABLE #tblApplicationAppManagerCount
(ApplicationId int,
AppManagerCount int)
insert into #tblApplicationAppManagerCount
SELECT
ApplicationId ,
COUNT(*) AS AppManagerCount
FROM UserApplicationLocale userLocale WITH (NOLOCK)
inner join ApplicationLocale appLocale WITH (NOLOCK) on(userLocale.ApplicationLocaleId = appLocale.Id)
WHERE applocale.LocaleId = 91
GROUP BY ApplicationLocaleId,ApplicationId



Update ut
SET AppManagerCount = amc.AppManagerCount
FROM #tblUserApplicationLocales ut
inner join #tblApplicationAppManagerCount amc ON (ut.ApplicationId = amc.ApplicationId)

SELECT 
ApplicationLocaleId 
,UserId 
,UserEmail 
,PreferredName 
,ApplicationId 
,ApplicationName 
,LocaleId
,LocaleCode 
,NativeName 
,EnglishName 
,NativeLanguageName 
,EnglishLanguageName 
,NativeCountryName 
,EnglishCountryName 
,UpdatedDate 
,UpdatedBy 
,TotalLabelsCount 
,TotalAssetsCount
,[UploadedAssetCount]
,[CanEdit]
,AppManagerCount
from #tblUserApplicationLocales
ORDER BY [CanEdit] desc,[ApplicationName], [LocaleCode]
END