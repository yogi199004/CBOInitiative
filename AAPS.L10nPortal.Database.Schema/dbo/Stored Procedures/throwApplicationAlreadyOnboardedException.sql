Create PROCEDURE [dbo].throwApplicationAlreadyOnboardedException
AS
BEGIN 
	;THROW 50000, 'ApplicationAlreadyOnboardedException', 255;
END
go