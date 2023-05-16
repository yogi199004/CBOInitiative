CREATE PROCEDURE [dbo].[spGetAppsLocaleSpecificKeyValues]
AS
BEGIN


DECLARE @isFirstSync BIT
DECLARE @lastSyncDate datetime

Select @isFirstSync = [Value] from SyncConfiguration WITH (NOLOCK) where [key] = 'IsFirstSync'


If @isFirstSync =1
BEGIN

	SELECT app.name as applicationname
	,L.Code AS LocaleCode
	,ark.Name AS 'ResourceKey'
	,arv.Value AS LocaleValue
	,arv.CreatedDate
	,arv.UpdatedDate
	,arv.CreatedBy
	,arv.UpdatedBy
	FROM [Application] app WITH (NOLOCK)
	join ApplicationResourceKey ark WITH (NOLOCK) ON(app.Id = ark.ApplicationId)
	join ApplicationResourceValue arv WITH (NOLOCK) ON(ark.Id = arv.ApplicationResourceKeyId) join Locale L WITH (NOLOCK) ON(arv.LocaleId = L.Id)
    where arv.value is not null
	Order BY app.Name, arv.LocaleId
END

ELSE
BEGIN
Select @lastSyncDate = [Value] from SyncConfiguration where [key] = 'LastSync'
	SELECT app.name as applicationname
        ,L.Code AS LocaleCode
        ,ark.Name AS 'ResourceKey'
        ,arv.Value AS LocaleValue
        ,arv.CreatedDate
        ,arv.UpdatedDate
        ,arv.CreatedBy
        ,arv.UpdatedBy
        FROM [Application] app WITH (NOLOCK)
        join ApplicationResourceKey ark WITH (NOLOCK) ON(app.Id = ark.ApplicationId)
        join ApplicationResourceValue arv WITH (NOLOCK) ON(ark.Id = arv.ApplicationResourceKeyId) join Locale L WITH (NOLOCK) ON(arv.LocaleId = L.Id)
        WHERE arv.value is not null and arv.LocaleId in
        (
        SELECT arv1.LocaleId  FROM ApplicationResourceKey ark1 WITH (NOLOCK)
        Inner JOIN   ApplicationResourceValue arv1 WITH (NOLOCK) ON(ark1.Id = arv1.ApplicationResourceKeyId)
        WHERE arv1.CreatedDate > = @lastsyncDate or arv1.UpdatedDate >= @lastsyncDate
        ) 
        AND app.Id in
        (
        SELECT ark1.ApplicationId  FROM ApplicationResourceKey ark1 WITH (NOLOCK)
        Inner JOIN   ApplicationResourceValue arv1 WITH (NOLOCK) ON(ark1.Id = arv1.ApplicationResourceKeyId)
        WHERE arv1.CreatedDate > = @lastsyncDate or arv1.UpdatedDate >= @lastsyncDate
        )

        Order BY app.Name, arv.LocaleId

END



END



