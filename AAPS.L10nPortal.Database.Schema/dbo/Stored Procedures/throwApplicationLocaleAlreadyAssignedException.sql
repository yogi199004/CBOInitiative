CREATE PROCEDURE [dbo].[throwApplicationLocaleAlreadyAssignedException]
AS
BEGIN 
	;THROW 50000, 'ApplicationLocaleAlreadyAssignedException', 255;
END