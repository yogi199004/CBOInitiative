Create PROCEDURE [dbo].throwRedisInstanceTwoFulledException
AS
BEGIN 
	;THROW 50000, 'RedisInstanceTwoFulledException', 255;
END
go