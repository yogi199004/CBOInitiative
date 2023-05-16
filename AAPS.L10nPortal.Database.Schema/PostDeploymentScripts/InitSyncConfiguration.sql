


 IF NOT EXISTS (SELECT * FROM [dbo].[SyncConfiguration] WHERE [Key] ='IsFirstSync')
		BEGIN
		 INSERT INTO [dbo].[SyncConfiguration]([Key],[Value],[Description])VALUES(N'IsFirstSync',1,
		  '1- full Sync, 0- Modified Only')
		END



 IF NOT EXISTS (SELECT * FROM [dbo].[SyncConfiguration] WHERE [Key] ='LastSync')
		BEGIN




		 INSERT INTO [dbo].[SyncConfiguration]([Key],[Value],[Description])VALUES(N'LastSync',getdate(),
		  'last Sync executed date time')

		END
   