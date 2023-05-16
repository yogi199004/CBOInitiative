CREATE PROCEDURE [dbo].[spGetAppsWithDeletedKey]

AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;
DECLARE @lastsyncDate datetime
SELECT @lastsyncDate =  [Value] FROM SyncConfiguration WITH (NOLOCK) where [key]= 'LastSync'
-- Insert statements for procedure here
SELECT app.name 
FROM AuditApplicationResourceKey auditAppResourceKey WITH (NOLOCK)
inner join [Application] app WITH (NOLOCK) on (auditAppResourceKey.AppId=app.Id)
--inner join ApplicationLocale appLocale on(app.Id = appLocale.ApplicationId)
--inner join locale l on (appLocale.LocaleId = l.Id)
WHERE auditAppResourceKey.DeletedDate>@lastsyncDate 
END
