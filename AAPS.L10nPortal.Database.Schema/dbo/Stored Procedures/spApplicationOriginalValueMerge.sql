create procedure spApplicationOriginalValueMerge
 @UserId uniqueidentifier,  
 @ApplicationLocaleId int,  
 @OriginalValues dbo.ResourceKeyValue readonly  
as  
begin  
  set nocount on;  
  
  declare @Now datetime = getutcdate();  
  declare @ApplicationId int;  
  declare @LocaleId int;  
  
  select  
    @ApplicationId = al.ApplicationId  
   ,@LocaleId = al.LocaleId  
  from dbo.ApplicationLocale al  WITH (NOLOCK)
  where al.Id = @ApplicationLocaleId;  
  
  declare @deletedKeys table (  
    Id int  
  )  
  declare @updatedKeyValues table (  
    Id int  
   ,Value nvarchar(max)  
  )  
  declare @assetupdatedKeyValues table (  
    Id int  
   ,Value nvarchar(max)  
  )  
  declare @values table (  
    Id int,  
    [Key] nvarchar(256),   
    OldTypeId int,  
    TypeId int,  
    OldValue nvarchar(max),  
    Value nvarchar(max),  
    OldDescription nvarchar(max),  
    Description nvarchar(max)  
  )  
   declare @assetvalues table (  
    Id int,  
    [Key] nvarchar(256),   
    OldTypeId int,  
    TypeId int,  
    OldValue nvarchar(max),  
    Value nvarchar(max),  
    OldDescription nvarchar(max),  
    Description nvarchar(max)  
  )  
  insert into @deletedKeys  
    select  
      ark.Id  
    from ApplicationResourceKey ark  
    left join @OriginalValues rkv  
      on rkv.[Key] = ark.Name  
    where ark.ApplicationId = @ApplicationId  
    and rkv.[Key] is null  
  
  delete ApplicationResourceValue  
  where ApplicationResourceKeyId in (select  
        Id  
      from @deletedKeys)  
  
  delete ApplicationResourceKey  
  where Id in (select  
        Id  
      from @deletedKeys)  
  
  insert @values   
  select ark.Id, ov.[Key], ark.TypeId, ov.TypeId, arv.Value, ov.Value, ark.Description, ov.Description from @OriginalValues ov  
    left join ApplicationResourceKey ark WITH (NOLOCK) on ov.[Key] = ark.Name and ark.ApplicationId = @ApplicationId
    left join ApplicationResourceValue arv WITH (NOLOCK) on ark.Id = arv.ApplicationResourceKeyId and arv.LocaleId = @LocaleId where ov.TypeId=1;
	
	insert @assetvalues   
  select ark.Id, ov.[Key], ark.TypeId, ov.TypeId, arv.Value, ov.Value, ark.Description, ov.Description from @OriginalValues ov  
    left join ApplicationResourceKey ark WITH (NOLOCK) on ov.[Key] = ark.Name and ark.ApplicationId = @ApplicationId 
    left join ApplicationResourceValue arv WITH (NOLOCK) on ark.Id = arv.ApplicationResourceKeyId and arv.LocaleId = @LocaleId where ov.TypeId=2; 
  
  update arv   
  set Value = v.Value, UpdatedBy = @UserId, UpdatedDate = getutcdate()   
  from @values v  
  inner join ApplicationResourceValue arv on arv.ApplicationResourceKeyId = v.Id and arv.LocaleId = @LocaleId  
  WHERE v.TypeId=1 and v.Id is not null and
  1=CASE WHEN v.OldValue is null THEN 1
         WHEN v.Value is null THEN 1
		 WHEN CONVERT(VARBINARY(MAX),LTRIM(RTRIM(v.OldValue))) <> CONVERT(VARBINARY(MAX),LTRIM(RTRIM(v.Value))) THEN 1 END;  

  update arv   
  set UpdatedBy = @UserId, UpdatedDate = getutcdate()   
  from @assetvalues v  
  inner join ApplicationResourceValue arv on arv.ApplicationResourceKeyId = v.Id and arv.LocaleId = @LocaleId  
  WHERE v.TypeId=2 and v.Id is not null and 
  1=CASE WHEN v.OldValue is null THEN 1
         WHEN v.Value is null THEN 1
		 WHEN CONVERT(VARBINARY(MAX),LTRIM(RTRIM(v.OldValue))) <> CONVERT(VARBINARY(MAX),LTRIM(RTRIM(v.Value))) THEN 1 END;  
  
   
  
  update ark   
  set TypeId = v.TypeId, Description = v.Description  
  from @values v  
  inner join ApplicationResourceKey ark on v.Id = ark.Id  
  where v.Id is not null;  

   update ark  
  set [Name] = v.[Key]   
  from @values v  
  inner join ApplicationResourceKey ark on ark.Id = v.Id and ark.ApplicationId = @ApplicationId and v.[Key] = ark.Name
  WHERE 1=CASE WHEN CONVERT(VARBINARY(MAX),LTRIM(RTRIM(ark.[Name]))) <> CONVERT(VARBINARY(MAX),LTRIM(RTRIM(v.[Key]))) THEN 1 END;  

  
  update ark  
  set [Name] = v.[Key]   
  from @assetvalues v  
  inner join ApplicationResourceKey ark on ark.Id = v.Id and ark.ApplicationId = @ApplicationId and v.[Key] = ark.Name
  WHERE 1=CASE WHEN CONVERT(VARBINARY(MAX),LTRIM(RTRIM(ark.[Name]))) <> CONVERT(VARBINARY(MAX),LTRIM(RTRIM(v.[Key]))) THEN 1 END;  




  merge ApplicationResourceKey ark  
  using (select* from @values where Id is null) src  
  on src.[Key] = ark.Name  
    and ark.ApplicationId = @ApplicationId  
  when not matched  
    then insert (Name, ApplicationId, TypeId, Description)  
        values (src.[Key], @ApplicationId, src.TypeId, src.Description)  
  output inserted.Id, src.Value into @updatedKeyValues;  
  
  merge ApplicationResourceValue arv  
  using (select * from @updatedKeyValues) src  
  on arv.ApplicationResourceKeyId = src.Id  
    and arv.LocaleId = @localeId  
  when not matched  
    then insert (ApplicationResourceKeyId, LocaleId, Value, CreatedBy, CreatedDate)  
        values (src.Id, @localeId, src.Value, @UserId, getutcdate());  
  
  merge ApplicationResourceKey ark  
  using (select* from @assetvalues where Id is null) src  
  on src.[Key] = ark.Name  
    and ark.ApplicationId = @ApplicationId  
  when not matched  
    then insert (Name, ApplicationId, TypeId, Description)  
        values (src.[Key], @ApplicationId, src.TypeId, src.Description)  
  output inserted.Id, src.Value into @assetupdatedKeyValues;  
  
  merge ApplicationResourceValue arv  
  using (select * from @assetupdatedKeyValues) src  
  on arv.ApplicationResourceKeyId = src.Id  
    and arv.LocaleId = @localeId  
  when not matched  
    then insert (ApplicationResourceKeyId, LocaleId, CreatedBy, CreatedDate)  
        values (src.Id, @localeId, @UserId, getutcdate());  

  update ApplicationLocale  
  set UpdatedDate = getutcdate()  
  where ApplicationId = @ApplicationId  
  and LocaleId = @localeId  
end   