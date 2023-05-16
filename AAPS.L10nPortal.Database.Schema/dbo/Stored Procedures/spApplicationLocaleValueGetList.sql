CREATE PROCEDURE spApplicationLocaleValueGetList
	@UserId uniqueidentifier,
	@ApplicationLocaleId int
AS
BEGIN
	SET NOCOUNT ON;

	declare @englishLocaleCode nvarchar(50) = 'en-US';

	declare @applicationId int;
	declare @localeId int;
	declare @englishLocaleId int;
	declare @englishValues table
	(
		[KeyId] int,
		[Key] nvarchar(256),
		[Value] nvarchar(max),
		[Description] nvarchar(max),
		[TypeId] int,
		[CreatedDate] datetime,
		[UpdatedDate] datetime
	);
	
	select 
		@applicationId = al.ApplicationId, 
		@localeId = al.LocaleId
	from 
		dbo.ApplicationLocale al WITH (NOLOCK)
	where 
		al.Id = @ApplicationLocaleId;

	select @englishLocaleId = l.Id 
	from dbo.Locale l WITH (NOLOCK)
	where l.Code = @englishLocaleCode;
	
	insert 
		@englishValues
	select 
		ark.Id, ark.Name, arv.Value, ark.Description, ark.TypeId, CreatedDate, arv.UpdatedDate
	from ApplicationResourceKey ark WITH (NOLOCK)
	left join ApplicationResourceValue arv WITH (NOLOCK) on ark.Id = arv.ApplicationResourceKeyId and arv.LocaleId = @englishLocaleId
	where 
		ark.ApplicationId = @applicationId;
	
	select 
		ev.[KeyId ]as [KeyId], 
		ev.[Key] as [Key], 
		ev.[Value] as [OriginalValue], 
		arv.[Value] as [TranslatedValue],
		ev.[Description] as [Description],
		ev.TypeId as TypeId,
		arv.LocaleId as [LocaleId],
		cast(isnull(arv.UpdatedDate, arv.CreatedDate) AS DATETIME) AS [UpdatedDate],
		cast(isnull(arv.UpdatedBy, arv.CreatedBy) AS uniqueidentifier) AS [UpdatedBy],
		cast(isnull(ev.UpdatedDate, ev.CreatedDate) AS DATETIME) AS [OriginalUpdatedDate]
	from @englishValues ev 
	left join ApplicationResourceValue arv WITH (NOLOCK) on arv.ApplicationResourceKeyId = ev.KeyId and arv.LocaleId = @localeId
	order by 
		ev.[Key];

END