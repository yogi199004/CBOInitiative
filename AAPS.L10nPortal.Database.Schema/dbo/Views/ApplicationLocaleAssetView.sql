CREATE VIEW [dbo].[ApplicationLocaleAssetView]
AS 
	SELECT 
		al.Id			AS [ApplicationLocaleId],
		ark.Id			AS [KeyId],
		ark.[Name]		AS [Key],
		arv.[Value]		AS [Value],
		cast(isnull(arv.UpdatedDate, arv.CreatedDate) AS DATETIME) AS [UpdatedDate],
		cast(isnull(arv.UpdatedBy, arv.CreatedBy) AS uniqueidentifier) AS [UpdatedBy]
	FROM [ApplicationResourceKey] ark WITH (NOLOCK)
	INNER JOIN [Application] a WITH (NOLOCK) ON ark.ApplicationId = a.Id
	INNER JOIN [ApplicationLocale] al WITH (NOLOCK) ON a.Id = al.ApplicationId
	LEFT JOIN [ApplicationResourceValue] arv WITH (NOLOCK) ON ark.Id = arv.ApplicationResourceKeyId and arv.LocaleId=al.LocaleId
	WHERE ark.TypeId = 2
