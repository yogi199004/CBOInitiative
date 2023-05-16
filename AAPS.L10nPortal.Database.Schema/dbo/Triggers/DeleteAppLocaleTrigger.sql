CREATE TRIGGER [DeleteAppLocaleTrigger]
   ON  ApplicationLocale
   AFTER delete
AS 
BEGIN
    insert into AuditApplicationAndLocale(AppId,LocaleId,LocaleDeleted,DeletedDate)       
    select D.ApplicationId, D.LocaleId, 'Y', GETDATE() from Deleted D
END