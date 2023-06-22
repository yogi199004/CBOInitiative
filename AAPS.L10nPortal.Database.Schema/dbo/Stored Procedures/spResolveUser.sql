

Create PROCEDURE spResolveUser
	-- Add the parameters for the stored procedure here
	@email varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


    -- Insert statements for procedure here
	IF exists(SELECT 1 from [User] where email  = @email)
	BEGIN

	  select id, email from [User] where email  = @email
	END
	ELSE
	   BEGIN

	    INSERT  INTO [User](
		Id
		,Email
		,CreatedDate

		)
		VALUES
		(
		 newid()
		,@email
		,getdate()
		)	
		 select id, email from [User] where email  = @email
END
END
GO
