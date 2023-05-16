CREATE FUNCTION [dbo].[fn_GetAppManagerCount]
(
	-- Add the parameters for the function here
	@appLocaleId int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @appManagerCount int
	DECLARE @appId int
	-- Add the T-SQL statements to compute the return value here
	SELECT @appId = ApplicationId from [applicationlocale] where id = @appLocaleId

	select @appManagerCount = count(1)
	FROM ApplicationLocale appLocale
	inner join UserApplicationLocale usLocale on (applocale.Id = usLocale.ApplicationLocaleId)
	where ApplicationId = @appId and LocaleId = 91


	-- Return the result of the function
	RETURN @appManagerCount

END
GO
