create procedure spApplicationLocaleValueMerge
	@UserId uniqueidentifier,
	@ApplicationLocaleId int,
	@TranslatedValues dbo.ApplicationLocaleValueCreateType readonly
as
begin
	set nocount on;

	declare @Now datetime = getutcdate();

	declare @LocaleValues table
	(
		ApplicationResourceKeyId int not null,
		LocaleId int not null,
		TranslatedValue nvarchar(max)
	);

	insert @LocaleValues
	select ark.Id, al.LocaleId, v.TranslatedValue from @TranslatedValues v 
	join ApplicationResourceKey ark WITH (NOLOCK) on ark.Name = v.[Key]
	join Application a WITH (NOLOCK) on ark.ApplicationId = a.Id
	join ApplicationLocale al WITH (NOLOCK) on a.Id = al.ApplicationId
	where al.Id = @ApplicationLocaleId;

	merge into 
		dbo.ApplicationResourceValue as target 
	using 
		(select ApplicationResourceKeyId, LocaleId, TranslatedValue from @LocaleValues) as source
	on 
		target.ApplicationResourceKeyId = source.ApplicationResourceKeyId and target.LocaleId = source.LocaleId
	when not matched by target then
		insert (ApplicationResourceKeyId, LocaleId, [Value], CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) 
		values (ApplicationResourceKeyId, LocaleId, TranslatedValue, @Now, @UserId, @Now, @UserId)
	when matched then
		update set 
		target.[Value] = source.TranslatedValue,
		target.UpdatedDate = CASE WHEN target.[Value] != source.TranslatedValue THEN  @Now  END ,
		target.UpdatedBy = @UserId;

	UPDATE ApplicationLocale
		SET UpdatedDate = @Now, UpdatedBy = @UserId
	WHERE Id = @ApplicationLocaleId

end