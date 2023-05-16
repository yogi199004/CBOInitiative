create procedure spApplicationLocaleAssetUpdate
	@userId uniqueidentifier,
	@applicationLocaleId int,
	@keyId int,
	@value nvarchar(max)
as
begin
	set nocount on;

	exec spUserApplicationLocaleCheckPermissions @userId = @userId, @applicationLocaleId = @applicationLocaleId

	declare @Now datetime = getutcdate();
	declare @localeId int;
	declare @englishLocaleCode nvarchar(50) = 'en-US';
	declare @englishLocaleId int;

	select @englishLocaleId = l.Id 
	from dbo.Locale l WITH (NOLOCK)
	where l.Code = @englishLocaleCode;

	select @localeId = LocaleId 
	from dbo.ApplicationLocale al WITH (NOLOCK)
	where al.Id = @applicationLocaleId;

	merge into 
		dbo.ApplicationResourceValue as target 
	using 
		(select * from (values(@keyId, @localeId, @value)) as s 
		(ApplicationResourceKeyId, LocaleId, [Value])) as source
	on 
		target.ApplicationResourceKeyId = source.ApplicationResourceKeyId and target.LocaleId = source.LocaleId
	when not matched by target then
		insert (ApplicationResourceKeyId, LocaleId, [Value], CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) 
		values (ApplicationResourceKeyId, LocaleId, [Value], @Now, @UserId, @Now, @UserId)
	when matched then
		update set 
		target.[Value] = source.[Value],
		target.UpdatedDate = @Now,
		target.UpdatedBy = @UserId;

	update dbo.ApplicationLocale
		set UpdatedDate = @Now, UpdatedBy = @userId
	where Id = @applicationLocaleId

	select  
		 alav.[ApplicationLocaleId]
		,alav.[KeyId]
		,alav.[Key]
		,alav.[Value]
		,alav.[UpdatedDate]
		,alav.[UpdatedBy]
		,cast(isnull(ev.UpdatedDate, ev.CreatedDate) AS DATETIME) AS [OriginalUpdatedDate]
	from dbo.ApplicationLocaleAssetView alav WITH (NOLOCK)
	left join  dbo.ApplicationResourceValue ev WITH (NOLOCK) ON ev.ApplicationResourceKeyId = alav.KeyId and ev.LocaleId = @englishLocaleId	
	where ApplicationLocaleId = @applicationLocaleId and KeyId = @keyId;
end