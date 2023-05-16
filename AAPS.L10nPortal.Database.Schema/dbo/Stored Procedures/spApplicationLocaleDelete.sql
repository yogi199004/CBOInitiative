create procedure dbo.spApplicationLocaleDelete
	@UserId uniqueidentifier,
	@ApplicationLocaleId int
as
begin
	set nocount on;

	declare @ApplicationId int;
	declare @LocaleId int;
	declare @LocaleCode nvarchar(10);
	DECLARE @IsAppMgr INT

	select @ApplicationId = al.ApplicationId, @LocaleCode = l.Code, @LocaleId = l.Id
	from dbo.ApplicationLocale al WITH (NOLOCK)
	join dbo.Locale l on l.Id = al.LocaleId
	where al.Id = @ApplicationLocaleId;

	
	EXEC @IsAppMgr = [dbo].[spCheckAppMgrPermissions] @userId = @userId, @applicationId=@applicationId
	IF NOT (@IsAppMgr =0)
	BEGIN
		EXEC [throwPermissionException]
	END

	if (@LocaleCode = 'en-US')
	begin
		exec dbo.throwPermissionException
	end

	delete arv from dbo.ApplicationResourceValue arv 
	join dbo.ApplicationResourceKey ark on ark.Id = arv.ApplicationResourceKeyId
	where ark.ApplicationId = @ApplicationId and arv.LocaleId = @LocaleId;

	delete from dbo.UserApplicationLocale where ApplicationLocaleId = @ApplicationLocaleId;

	delete from dbo.ApplicationLocale where Id = @ApplicationLocaleId;
end