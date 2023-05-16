CREATE PROCEDURE spApplicationLocaleAssetGet
	@userId					UNIQUEIDENTIFIER,
	@applicationLocaleId	INT,
	@keyId					INT
AS
BEGIN
	SET NOCOUNT ON;

	EXEC spUserApplicationLocaleCheckPermissions @userId = @userId, @applicationLocaleId = @applicationLocaleId

	declare @englishLocaleCode nvarchar(50) = 'en-US';
	declare @englishLocaleId int;

	select @englishLocaleId = l.Id 
	from dbo.Locale l 
	where l.Code = @englishLocaleCode;

	SELECT 
		 alav.[ApplicationLocaleId]
		,alav.[KeyId]
		,alav.[Key]
		,alav.[Value]
		,alav.[UpdatedDate]
		,alav.[UpdatedBy]
		,cast(isnull(ev.UpdatedDate, ev.CreatedDate) AS DATETIME) AS [OriginalUpdatedDate]
	FROM [ApplicationLocaleAssetView] AS alav WITH (NOLOCK)
	LEFT JOIN dbo.ApplicationResourceValue ev WITH (NOLOCK) ON ev.ApplicationResourceKeyId = alav.KeyId and ev.LocaleId = @englishLocaleId
	WHERE alav.ApplicationLocaleId = @applicationLocaleId and KeyId = @keyId;
END