CREATE PROCEDURE [dbo].[spCheckAdminPermissions]
	@userId					UNIQUEIDENTIFIER
AS
BEGIN
	IF EXISTS(SELECT * FROM [UserRolemapping] ur WITH (NOLOCK) 
	INNER JOIN RoleMaster r WITH (NOLOCK) on ur.roleId=r.Id WHERE ur.GUId=@userId and r.Id=1 and ur.Active=1 )
	RETURN 0
	ELSE RETURN -1	
END