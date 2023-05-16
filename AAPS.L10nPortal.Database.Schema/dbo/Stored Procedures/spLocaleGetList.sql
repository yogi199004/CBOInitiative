CREATE PROCEDURE [dbo].[spLocaleGetList]
AS
	SELECT Id, Code, NativeName, EnglishName FROM [Locale] WITH (NOLOCK) order by EnglishName 