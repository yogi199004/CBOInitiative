CREATE PROCEDURE [dbo].[throwPermissionException]
AS
BEGIN 
	;THROW 50000, 'PermissionException', 255;
END