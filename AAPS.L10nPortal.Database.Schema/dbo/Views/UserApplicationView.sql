CREATE VIEW [dbo].[UserApplicationView]
AS 
	SELECT u.Id AS [UserId], a.Id AS [ApplicationId], a.[Name] AS [ApplicationName],
		CASE u.Id
			WHEN NULL THEN 0
			ELSE 1
		END AS [CanManage]
	FROM [Application] AS a WITH (NOLOCK)
	INNER JOIN [ApplicationLocale] AS al WITH (NOLOCK) ON al.ApplicationId = a.Id
	INNER JOIN [Locale] AS l WITH (NOLOCK) ON l.Id = al.LocaleId AND l.Code = 'en-US'
	LEFT JOIN [UserApplicationLocale] AS ual WITH (NOLOCK) ON ual.ApplicationLocaleId = al.Id
	LEFT JOIN [User] AS u WITH (NOLOCK) ON u.Id = ual.UserId