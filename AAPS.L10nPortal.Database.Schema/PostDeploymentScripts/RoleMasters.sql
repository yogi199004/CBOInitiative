MERGE INTO [dbo].[Rolemaster] AS a
USING (VALUES
(1	,N'Portal Admin','yodubey@deloitte.com',Getutcdate(),'yodubey@deloitte.com',Getutcdate()),
(2	,N'User','yodubey@deloitte.com',Getutcdate(),'yodubey@deloitte.com',Getutcdate())

) AS t ([Id],[Name] ,[Createdby],[CreatedDate], [Updatedby],[UpdatedDate])
ON a.[Name] = t.[Name]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Id],[Name] ,[Createdby],[CreatedDate], [Updatedby],[UpdatedDate])
    VALUES ([Id],[Name] ,[Createdby],[CreatedDate], [Updatedby],[UpdatedDate])
WHEN MATCHED THEN 
UPDATE SET 	a.[Id]  =t.[Id],
	a.[Name] = t.[Name] ,
	a.[Createdby] =t.[Createdby],
	a.[CreatedDate] =t.[CreatedDate],
    a.[Updatedby] =t.[Updatedby],
	a.[UpdatedDate] =t.[UpdatedDate]

WHEN NOT MATCHED BY SOURCE THEN
    DELETE;


