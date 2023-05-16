CREATE PROCEDURE [dbo].[spGetDeletedLocales]

AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;
DECLARE @lastsyncDate datetime
SELECT @lastsyncDate =  [Value] FROM SyncConfiguration WITH (NOLOCK) where [key]= 'LastSync'
-- Insert statements for procedure here
select app.name,l.Code  from AuditapplicationandLocale audit WITH (NOLOCK)
inner join application app WITH (NOLOCK) on audit.AppId=app.Id
inner join locale l WITH (NOLOCK) on audit.LocaleId = l.Id where audit.DeletedDate>@lastsyncDate and audit.LocaleDeleted = 'Y'
END