CREATE PROCEDURE [dbo].[throwLocaleNotFoundException]
AS
BEGIN 
	;THROW 50000, 'LocaleNotFoundException', 255;
END