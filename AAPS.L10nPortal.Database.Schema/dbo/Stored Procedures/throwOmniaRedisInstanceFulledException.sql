Create PROCEDURE [dbo].throwOmniaRedisInstanceFulledException
AS
BEGIN 
	;THROW 50000, 'OmniaRedisInstanceFulledException', 255;
END
go