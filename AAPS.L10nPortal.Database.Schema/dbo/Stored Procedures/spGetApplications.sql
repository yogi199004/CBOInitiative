Create PROCEDURE [dbo].[spGetApplications]

AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here
select distinct app.Name, app.id from application app WITH (NOLOCK)
inner join applicationlocale appLocale WITH (NOLOCK) on(app.id = appLocale.applicationid)
inner join locale l WITH (NOLOCK) on(l.Id = appLocale.LocaleId)
END