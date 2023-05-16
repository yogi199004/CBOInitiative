create table UserRolemapping(
GUId  UNIQUEIDENTIFIER,
Email nvarchar(max),
RoleId int,
Active bit,
Createdby nvarchar(max),
CreatedDate Datetime ,
Updatedby nvarchar(max),
UpdatedDate Datetime,
CONSTRAINT [FK_RoleID] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[RoleMaster] ([Id]) )