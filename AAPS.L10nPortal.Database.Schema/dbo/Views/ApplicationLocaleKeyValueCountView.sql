CREATE VIEW [dbo].[ApplicationLocaleKeyValueCountView]
AS

    SELECT ApplicationId, ISNULL([1], 0) AS 'LocalesCount', ISNULL([2], 0) AS 'AssetsCount' FROM
    (    
        SELECT ark.ApplicationId, ark.TypeId AS [TypeId], COUNT(ark.Id) [Count] FROM [ApplicationResourceKey] AS ark WITH (NOLOCK)
        GROUP BY ark.ApplicationId, ark.TypeId
    ) p  
    PIVOT  
    (  
        SUM ([Count])  
        FOR TypeId IN  
            ([1], [2])
    ) AS pvt;