CREATE PROCEDURE spApplicationLocaleAssetKeysWithAssetGetList
	@userId					UNIQUEIDENTIFIER,
	@applicationLocaleId	INT
AS
BEGIN
	SET NOCOUNT ON;

	EXEC spUserApplicationLocaleCheckPermissions @userId = @userId, @applicationLocaleId = @applicationLocaleId

	declare @englishLocaleCode nvarchar(50) = 'en-US';
	declare @englishLocaleId int;

	select @englishLocaleId = l.Id 
	from dbo.Locale l WITH (NOLOCK)
	where l.Code = @englishLocaleCode;

	SELECT 
		 alav.[ApplicationLocaleId]
		,alav.[KeyId]
		,alav.[Key]
		,alav.[Value]
		,alav.[UpdatedDate]
		,alav.[UpdatedBy]
		,alav.UpdatedDate AS [OriginalUpdatedDate]
	FROM dbo.ApplicationLocaleAssetView alav WITH (NOLOCK)
	WHERE alav.ApplicationLocaleId = @applicationLocaleId and Len(value) >0
	ORDER BY alav.[Key]
END