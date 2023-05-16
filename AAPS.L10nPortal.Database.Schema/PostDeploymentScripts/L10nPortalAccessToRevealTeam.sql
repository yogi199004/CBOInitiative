/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
 ---Insert User 
USE L10nPortal
GO

---L10NPortal Access script
IF NOT EXISTS (
             SELECT 1
             FROM [dbo].[User]
             WHERE [Email] = 'juavalencia@deloitte.com'
             )
BEGIN
       INSERT INTO [dbo].[User]
       VALUES (
             '00000000-0000-0000-0000-000000004086'
             ,'juavalencia@deloitte.com'
             ,'Valencia, Juan'
             ,NULL
             ,NULL
             ,NULL
             ,NULL
             )
END

          --Jaun user for en-us
       DECLARE @uid1 UNIQUEIDENTIFIER;
       DECLARE @applicationLocaleid INT;
       DECLARE @applicationid INT;
       DECLARE @localeid INT;

       SELECT @localeid = l.id
       FROM Locale l
       WHERE l.Code = 'en-US'

       SELECT @applicationid = a.id
       FROM [Application] a
       WHERE a.[Name] = 'Reveal'

       SELECT @uid1 = u.Id
       FROM [User] u
       WHERE u.Email = 'juavalencia@deloitte.com';

IF NOT EXISTS (
             SELECT 1
             FROM [dbo].[ApplicationLocale]
             WHERE [ApplicationId] = @applicationid and LocaleId = @localeid
             )
BEGIN
       INSERT INTO [dbo].[ApplicationLocale] ( [ApplicationId] ,[LocaleId],[Active] ,[CreatedDate] ,[CreatedBy],[UpdatedDate] ,[UpdatedBy] )
       VALUES ( @applicationid,@localeid ,1,GETDATE(),@uid1,GETDATE(),@uid1 )

       SELECT @applicationLocaleid = al.id
       FROM ApplicationLocale al
       WHERE ApplicationId = @applicationid
             AND LocaleId = @localeid

       INSERT UserApplicationLocale ( UserId,ApplicationLocaleId)
       VALUES (@uid1,@applicationLocaleid)

End




