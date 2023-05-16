--SET CONTAINMENT = PARTIAL
--ALTER DATABASE L10nPortal SET CONTAINMENT = PARTIAL
--GO
--Add User
IF NOT EXISTS (SELECT * 
                FROM [sys].[database_principals]
                WHERE [type] = 'S' AND name = N'L10nPortal_User')
	BEGIN
		CREATE USER [L10nPortal_User] WITH PASSWORD = 'Portal123!' 
	
		EXEC sp_addrolemember 'db_datareader', 'L10nPortal_User'
	
		EXEC sp_addrolemember 'db_datawriter', 'L10nPortal_User'
	
		GRANT EXECUTE TO [L10nPortal_User]
	END

GO