Create PROCEDURE [dbo].[spGetAppsMetaData]
-- Add the parameters for the stored procedure here
--@appName Varchar(100)
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.

DECLARE @isFirstSync BIT
DECLARE @lastSyncDate datetime

Select @isFirstSync = [Value] from SyncConfiguration WITH (NOLOCK) where [key] = 'IsFirstSync'

If @isFirstSync =1
BEGIN
	SELECT app.Name as 'AppName'
	,l.NativeName as 'Language'
	,l.Code as 'LocaleCode'
	,l.NativeName as 'NativeName'
	,l.EnglishName as 'EnglishName'
	,l.NativeLanguageName as 'NativeLanguageName'
	,l.EnglishLanguageName as'EnglishLanguageName'
	,l.NativeCountryName as 'NativeCountryName'
	,l.EnglishCountryName as 'EnglishCountryName'
	,applocale.UpdatedDate as 'LastModifiedDate'
	,applocale.CreatedDate as 'LocaleCreatedDate'
	FROM application app WITH (NOLOCK)
	inner join applicationlocale applocale WITH (NOLOCK) on (app.id = applocale.Applicationid)
	inner join locale l WITH (NOLOCK) on (l.id = applocale.Localeid)
	ORDER BY app.Name

END
ELSE

BEGIN

	SELECT @lastsyncDate =  [Value] FROM SyncConfiguration WITH (NOLOCK) where [key]= 'LastSync'

	SELECT app.Name as 'AppName'
	,l.NativeName as 'Language'
	,l.Code as 'LocaleCode'
	,l.NativeName as 'NativeName'
	,l.EnglishName as 'EnglishName'
	,l.NativeLanguageName as 'NativeLanguageName'
	,l.EnglishLanguageName as'EnglishLanguageName'
	,l.NativeCountryName as 'NativeCountryName'
	,l.EnglishCountryName as 'EnglishCountryName'
	,applocale.UpdatedDate as 'LastModifiedDate'
	,applocale.CreatedDate as 'LocaleCreatedDate'
	FROM APPLICATION app WITH (NOLOCK)
	inner join applicationlocale applocale WITH (NOLOCK) on (app.id = applocale.Applicationid)
	inner join locale l WITH (NOLOCK) on (l.id = applocale.Localeid)
	WHERE app.Id in 
	(
	SELECT applicationid FROM applicationlocale WITH (NOLOCK) WHERE CreatedDate > @lastSyncDate or UpdatedDate >@lastSyncDate
	UNION
	SELECT AppId from AuditApplicationAndLocale WITH (NOLOCK) WHERE DeletedDate > @lastSyncDate and LocaleDeleted = 'Y'
	)
	ORDER BY app.Name

END



END