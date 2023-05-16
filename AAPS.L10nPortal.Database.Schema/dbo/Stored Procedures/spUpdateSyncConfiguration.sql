CREATE PROCEDURE [dbo].[UpdateSyncConfiguration]

AS

BEGIN
UPDATE SyncConfiguration
SET [Value]= '0'
WHERE [KEY] = 'IsFirstSync'


UPDATE SyncConfiguration
SET [Value]= Getdate()
WHERE [KEY] = 'LastSync'

END