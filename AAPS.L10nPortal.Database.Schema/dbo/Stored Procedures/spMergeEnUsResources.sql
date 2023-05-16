CREATE PROCEDURE [dbo].[spMergeEnUsResources] 
    @appId INT,
    @resourceKeyValue ResourceKeyValue READONLY
AS
BEGIN 
    SET NOCOUNT ON;
    
    DECLARE @localeId INT
    DECLARE @localeCode nvarchar(16) = N'en-us'
    DECLARE @systemUserId UNIQUEIDENTIFIER = CONVERT(UNIQUEIDENTIFIER, '00000000-0000-0000-0000-000000000000')
    DECLARE @errorMessage nvarchar(500)

    SELECT @localeId = Id
      FROM Locale WITH (NOLOCK)
     WHERE Code = @localeCode

    IF @localeId IS NULL
    BEGIN
        SET @errorMessage = 'There is no Locale with code = ' + @localeCode
        ;THROW 50000, @errorMessage, 255;
    END

    IF NOT EXISTS (SELECT 1 FROM [User] WITH (NOLOCK) WHERE Id = @systemUserId)
    BEGIN
        SET @errorMessage = 'There is no system User in the User table'
        ;THROW 50000, @errorMessage, 255;
    END

    IF NOT EXISTS (SELECT 1 FROM Application WITH (NOLOCK) WHERE Id = @appId)
    BEGIN
        SET @errorMessage = 'There is no Application with appId = ' + CAST(@appId AS VARCHAR(20))
        ;THROW 50000, @errorMessage, 255;
    END

    IF EXISTS (SELECT 1 
                 FROM @resourceKeyValue as kvs 
                      LEFT JOIN ApplicationResourceKeyType rkt WITH (NOLOCK) ON rkt.Id = kvs.TypeId
                WHERE rkt.Id IS NULL
              )
    BEGIN
        SET @errorMessage = 'There are not existed types'
        ;THROW 50000, @errorMessage, 255;
    END

    BEGIN TRY
        BEGIN TRANSACTION

            DECLARE @deletedKeys TABLE (Id INT)
            DECLARE @updatedKeyValues TABLE (Id INT, Value NVARCHAR (MAX))

            INSERT INTO @deletedKeys
            SELECT ark.Id
              FROM ApplicationResourceKey ark WITH (NOLOCK)
                   LEFT JOIN @resourceKeyValue rkv ON rkv.[Key] = ark.Name
             WHERE ark.ApplicationId = @appId
               AND rkv.[Key] IS NULL
             
             DELETE ApplicationResourceValue WHERE ApplicationResourceKeyId IN (SELECT Id FROM @deletedKeys)

             DELETE ApplicationResourceKey WHERE Id IN (SELECT Id FROM @deletedKeys)

             MERGE ApplicationResourceKey ark
             USING (SELECT * FROM @resourceKeyValue) src
                ON src.[Key] = ark.Name AND ark.ApplicationId = @appId
              WHEN MATCHED THEN
                   UPDATE SET
                       TypeId = src.TypeId,
                       Description = src.Description
              WHEN NOT MATCHED THEN
                   INSERT (Name, ApplicationId, TypeId, Description)
                   VALUES (src.[Key], @appId, src.TypeId, src.Description)
            OUTPUT inserted.Id, src.Value INTO @updatedKeyValues;
 
             MERGE ApplicationResourceValue arv
             USING (SELECT * FROM @updatedKeyValues) src
                ON arv.ApplicationResourceKeyId = src.Id AND arv.LocaleId = @localeId
              WHEN MATCHED THEN
                   UPDATE SET
                       Value = src.Value
              WHEN NOT MATCHED THEN
                   INSERT (ApplicationResourceKeyId, LocaleId, Value, CreatedBy, CreatedDate)
                   VALUES (src.Id, @localeId, src.Value, @systemUserId, GETUTCDATE());

             UPDATE ApplicationLocale SET
                 UpdatedDate = GETUTCDATE()
             WHERE ApplicationId = @appId
               AND LocaleId = @localeId

        COMMIT TRANSACTION
    END TRY

    BEGIN CATCH
        IF XACT_STATE() <> 0
        BEGIN
            ROLLBACK TRANSACTION
        END;
        THROW;
    END CATCH;
   
END