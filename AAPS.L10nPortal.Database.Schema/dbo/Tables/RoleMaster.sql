create table RoleMaster(
Id int ,
Name nvarchar(max),
Createdby nvarchar(max),
CreatedDate Datetime ,
Updatedby nvarchar(max),
UpdatedDate Datetime,
CONSTRAINT [PK_RoleID] PRIMARY KEY CLUSTERED (Id ASC))