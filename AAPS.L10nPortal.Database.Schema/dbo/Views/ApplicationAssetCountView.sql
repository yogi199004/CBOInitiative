CREATE VIEW [dbo].[ApplicationAssetCountView]
AS

SELECT a.Id as ApplicationId, al.LocaleId, count(arv.Value) as 'UploadedAssetCount' from [Application] a WITH (NOLOCK)
inner join ApplicationLocale al WITH (NOLOCK) on (a.Id = al.ApplicationId)
left join ApplicationResourceKey ark WITH (NOLOCK) on (a.Id = ark.ApplicationId)
left join ApplicationResourceValue arv WITH (NOLOCK) on (ark.Id = arv.ApplicationResourceKeyId and arv.LocaleId = al.LocaleId)
WHERE ark.TypeId = 2 and len(arv.Value)>0
GROUP BY a.Id, al.LocaleId

GO
