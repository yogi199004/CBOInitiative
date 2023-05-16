
Create PROCEDURE [dbo].[spUserroleMappingGet]
 @UserId nvarchar (max)
    

AS 
BEGIN
    SET NOCOUNT ON

    
      if exists(select * from UserRoleMapping ur WITH (NOLOCK) inner join RoleMaster r WITH (NOLOCK) on ur.roleId=r.Id where ur.GUId=@UserId and r.Id=1 and ur.Active=1)
      begin
			SELECT CAST(1 AS BIT) AS [Value]
      end
      else
      begin
                
          SELECT CAST(0 AS BIT) AS [Value]    
      end   
                

       
     

END


