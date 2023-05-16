using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using L10N.API.Common;
using L10N.API.Contracts;
using L10N.API.Model;
using Microsoft.Azure.Cosmos;

namespace L10N.API.BAL
{
    public class APIService : IAPIService
    {

        //public async Task<StoredProcedureExecuteResponse<MetaData>> getAppMetaData(Container containerClient, string parameterAppName)
        //{
        //    parameterAppName = parameterAppName + "_MetaData";
        //    PartitionKey partitionKey = new PartitionKey(parameterAppName);

        //    StoredProcedureExecuteResponse<MetaData> result =
        //        await containerClient.Scripts.ExecuteStoredProcedureAsync<MetaData> ("sp_GetMetaData", partitionKey, null, null);
        //    return result;

        //}

        public async Task<List<IEnumerable<MetaDataModel>>> getAppMetaData(Container containerClient, string appName)
        {
            appName = (appName + "_MetaData").ToLower();
            //var sqlQueryText = "Select value c.MetaData FROM c where c.AppName = "+ appName + "_MetaData";

            var sqlQueryText = "Select value c.MetaData FROM c where c.AppName =@appName";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText).WithParameter("@appName", appName);
            FeedIterator<IEnumerable<MetaDataModel>> myQuery = containerClient.GetItemQueryIterator<IEnumerable<MetaDataModel>>(queryDefinition, requestOptions: new QueryRequestOptions
            {
                ConsistencyLevel = ConsistencyLevel.Eventual,
                MaxItemCount = 5000,
                PartitionKey = new PartitionKey(appName)
            });

            FeedResponse<IEnumerable<MetaDataModel>> response = await myQuery.ReadNextAsync();

            return response.ToList(); ;
            //FeedResponse<IEnumerable<MetaDataModel>> currentResultSet = await myQuery.ReadNextAsync();
            //return currentResultSet;

        }



        public async Task<List<KeyValues>> getAppKeyValuesData(Container containerClient, string appName, string locale)
        {
            appName = (appName + "_" + locale).ToLower();
            //var sqlQueryText = "Select value c.MetaData FROM c where c.AppName = "+ appName + "_MetaData";

            var sqlQueryText = "SELECT  c.AllKeyValues FROM c where c.AppName =@appName";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText).WithParameter("@appName", appName);
            FeedIterator<KeyValues> myQuery = containerClient.GetItemQueryIterator<KeyValues>(queryDefinition, requestOptions: new QueryRequestOptions
            {
                ConsistencyLevel = ConsistencyLevel.Eventual,
                MaxItemCount = 5000,
                PartitionKey = new PartitionKey(appName)
            });

            FeedResponse<KeyValues> response = await myQuery.ReadNextAsync();
            return response.ToList<KeyValues>();
            //FeedResponse<IEnumerable<MetaDataModel>> currentResultSet = await myQuery.ReadNextAsync();
            //return currentResultSet;
        }




        public async Task<FeedResponse<string>> getAppIndividualKeyValueData(Container containerClient, string appName, string locale, string keyName)
        {
            appName = appName + "_" + locale;
            //var sqlQueryText = "Select value c.MetaData FROM c where c.AppName = "+ appName + "_MetaData";

            var sqlQueryText = "SELECT  Value c.keyValues [@keyName] FROM c where c.AppName =@appName";



            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText).WithParameter("@appName", appName).WithParameter("@keyName", keyName);
            //QueryDefinition queryDefinition1 = new QueryDefinition(sqlQueryText).WithParameter("@keyName", keyName);
            FeedIterator<string> myQuery = containerClient.GetItemQueryIterator<string>(queryDefinition, requestOptions: new QueryRequestOptions
            {
                ConsistencyLevel = ConsistencyLevel.Eventual,
                MaxItemCount = 5000,


            });




            FeedResponse<string> response = await myQuery.ReadNextAsync();

            return response;
            //FeedResponse<IEnumerable<MetaDataModel>> currentResultSet = await myQuery.ReadNextAsync();
            //return currentResultSet;
        }


        public BlobContainerClient GetContainer(string blobName, string SAKey)
        {
            //var keyVaultProvider = new AzureKeyVaultDataProvider();
            //var secret = Task.Run(() => keyVaultProvider.GetSecretAsync(ConfigValues.KeyVaultURI, SAKey)).Result;
            var connectionStringTemplate = ConfigValues.StorageAccountConnectionString;

            var storageConnectionString = connectionStringTemplate.Replace("${AccountKey}", SAKey);



            //var storageConnectionString = connectionStringTemplate.Replace("${AccountKey}", "sAkh1o3HrJNUEMmK+yIcnmvGKxCgHYGKRJbblmuMAfRuH0BviqFBhPsxzJ6IlOB1hDoalTXkLAw9drppp/4BFw==");
            BlobContainerClient blobContainerClient = new BlobContainerClient(storageConnectionString, ConfigValues.ContainerName);

            return blobContainerClient;
        }

        public string GetSASUrl(string blobName, string SAKey)
        {
            BlobContainerClient container = GetContainer(blobName, SAKey);

            return GetBlobSasUri(container, blobName);

        }

        public string GetBlobSasUri(BlobContainerClient container, string blobName, string storedPolicyName = null)
        {
            BlobClient blobClient = container.GetBlobClient(blobName);

            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = container.Name,
                BlobName = blobClient.Name,
                Resource = "b"// this is to notify SAS for Blobs
            };

            if (storedPolicyName == null)
            {
                sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(24);
                sasBuilder.SetPermissions(BlobSasPermissions.Read);
            }
            else
            {
                sasBuilder.Identifier = storedPolicyName;
            }

            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri.ToString();
        }



    }
}
