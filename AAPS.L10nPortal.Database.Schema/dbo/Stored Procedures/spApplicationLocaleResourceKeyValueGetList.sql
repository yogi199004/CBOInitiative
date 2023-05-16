CREATE PROCEDURE [dbo].[spApplicationLocaleResourceKeyValueGetList]
	@applicationId INT,
	@localeId INT
AS
	SELECT
		[ApplicationId]
		,[LocaleId]
		,[LocaleCode]
		,[ResourceKey]
		,[ResourceKeyTypeId]
		,[ResourceValue]
		,[UpdatedDate]
		,[UpdatedBy]
	FROM [ApplicationLocaleResourceKeyValueView] WITH (NOLOCK)
	WHERE [ApplicationId] = @applicationId AND [LocaleId] = @localeId