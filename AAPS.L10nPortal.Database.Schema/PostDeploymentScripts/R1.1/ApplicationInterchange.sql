USE L10nPortal
Go
--swapping will happen between app1 and app2
DECLARE @app1 NVARCHAR  (128)='';
DECLARE @app2 NVARCHAR  (128)='';
DECLARE @targetpositionapp1 int=16;
DECLARE @targetpositionapp2 int=13;
DECLARE @applicationId1 int=0;
DECLARE @applicationId2 int=0;
DECLARE @swapId int=46;

--IF ('$(Environment)' = 'DEV' OR '$(Environment)' = 'QA' OR '$(Environment)' = 'LOAD')
--BEGIN
--    SELECT @applicationId1=ApplicationId from [dbo].[Application] WHERE Name=@app1;
--    SELECT @applicationId2=ApplicationId from [dbo].[Application] WHERE Name=@app2;

--    IF (@applicationId1 <> @targetpositionapp1 and @applicationId2 <> @targetpositionapp2) 
--    BEGIN
--     UPDATE [dbo].[ApplicationLocale] SET ApplicationId=@swapId WHERE ApplicationId=@applicationId1
--     UPDATE [dbo].[ApplicationLocale] SET ApplicationId=@applicationId1 WHERE ApplicationId=@applicationId2
--     UPDATE [dbo].[ApplicationLocale] SET ApplicationId=@applicationId2 WHERE ApplicationId=@swapId
 
--     UPDATE [dbo].[ApplicationResourceKey] SET ApplicationId=@swapId WHERE ApplicationId=@applicationId1
--     UPDATE [dbo].[ApplicationResourceKey] SET ApplicationId=@applicationId1 WHERE ApplicationId=@applicationId2
--     UPDATE [dbo].[ApplicationResourceKey] SET ApplicationId=@applicationId2 WHERE ApplicationId=@swapId
--    END

--END