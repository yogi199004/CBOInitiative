using Azure.Storage.Blobs;
using L10N.API.Model;
using Microsoft.Azure.Cosmos;

namespace L10N.API.Contracts
{
    public interface IAPIService
    {
        //Task<FeedResponse<IEnumerable<MetaDataModel>>> getAppMetaData(Container containerClient, string appName);
        Task<List<IEnumerable<MetaDataModel>>> getAppMetaData(Container containerClient, string appName);

        //Task<StoredProcedureExecuteResponse<MetaData>> getAppMetaData(Container containerClient, string parameterAppName);

        Task<List<KeyValues>> getAppKeyValuesData(Container containerClient, string appName, string locale);
        //Task<StoredProcedureExecuteResponse<KeyValues>> getAppKeyValuesData(Container containerClient, string appName, string locale);


        Task<FeedResponse<string>> getAppIndividualKeyValueData(Container containerClient, string appName, string locale, string keyName);

        BlobContainerClient GetContainer(string blobName, string SAKey);

        string GetSASUrl(string blobName, string SAKey);

        string GetBlobSasUri(BlobContainerClient container, string blobName, string storedPolicyName = null);
    }
}
