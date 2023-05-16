CREATE VIEW [dbo].[ApplicationLocaleResourceKeyValueView]
AS
	SELECT 
		al.[ApplicationId]
	--    ,al.[ApplicationName]
		,al.[LocaleId]
		,al.[LocaleCode]
		,ark.[Name]         AS [ResourceKey]
		,ark.[TypeId]       AS [ResourceKeyTypeId]
		,arv.[Value]        AS [ResourceValue]
		,cast(isnull(arv.UpdatedDate, arv.CreatedDate) AS DATETIME) AS [UpdatedDate]
		,cast(isnull(arv.UpdatedBy, arv.CreatedBy) AS uniqueidentifier) AS [UpdatedBy]
	FROM [ApplicationLocaleView] AS al WITH (NOLOCK)
	INNER JOIN [ApplicationResourceKey] AS ark WITH (NOLOCK) ON ark.ApplicationId = al.ApplicationId
	INNER JOIN [ApplicationResourceValue] AS arv WITH (NOLOCK) ON arv.ApplicationResourceKeyId = ark.Id AND arv.LocaleId = al.LocaleId