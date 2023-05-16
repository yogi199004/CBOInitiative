IF '$(Environment)' IN ('STAGE','PROD','BCP')
Begin

MERGE INTO [dbo].[UserRolemapping] AS a
USING (VALUES
('00000000-0000-0000-0000-00000011a4e4'	,N'skolhatkar@deloitte.com',1,1,'yodubey@deloitte.com',Getutcdate(),'yodubey@deloitte.com',Getutcdate()),
('00000000-0000-0000-0000-000000002973'	,N'vkotik@deloitte.com',1,1,'yodubey@deloitte.com',Getutcdate(),'yodubey@deloitte.com',Getutcdate()),
('00000000-0000-0000-0000-00000012dfdb'	,N'nagkaranam@deloitte.com',1,1,'yodubey@deloitte.com',Getutcdate(),'yodubey@deloitte.com',Getutcdate())

) AS t ([GUId],[Email] ,[RoleId],[Active],[Createdby],[CreatedDate], [Updatedby],[UpdatedDate])
ON a.[GUId] = t.[GUId]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([GUId],[Email] ,[RoleId],[Active],[Createdby],[CreatedDate], [Updatedby],[UpdatedDate])
    VALUES ([GUId],[Email] ,[RoleId],[Active],[Createdby],[CreatedDate], [Updatedby],[UpdatedDate])
WHEN MATCHED THEN 
UPDATE SET 	a.[GUId]  =t.[GUId],
	a.[Email] = t.[Email] ,
	a.[RoleId] =t.[RoleId],
	a.[Active] = t.[Active],
	a.[Createdby] =t.[Createdby],
	a.[CreatedDate] =t.[CreatedDate],
    a.[Updatedby] =t.[Updatedby],
	a.[UpdatedDate] =t.[UpdatedDate]

WHEN NOT MATCHED BY SOURCE THEN
    Delete;

	END
	


