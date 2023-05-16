CREATE PROCEDURE [dbo].[throwApplicationNotFoundException]
AS
BEGIN 
	;THROW 50000, 'ApplicationNotFoundException', 255;
END