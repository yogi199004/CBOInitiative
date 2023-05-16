CREATE PROCEDURE [dbo].[spGetKeyValuesForAppWithDeletedKey]
(
@appName varchar(100)
)
AS
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
        WHERE arv.value is not null and app.Name =@appName
        Order BY app.Name, arv.LocaleId
END