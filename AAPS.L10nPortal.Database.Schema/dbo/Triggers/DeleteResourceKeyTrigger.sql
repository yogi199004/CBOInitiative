CREATE TRIGGER [DeleteResourceKeyTrigger]
   ON  ApplicationResourceKey
   AFTER delete
AS 
BEGIN
    insert into AuditApplicationResourceKey(AppId, DeletedDate)       
    select D.ApplicationId,GETDATE() from Deleted D
END