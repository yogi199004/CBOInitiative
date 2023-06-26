using CAPPortal.Contracts.Models;
using CAPPortal.Secrets;
using CAPPortal.Common;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;

namespace CAPPortal.Bal.AzureBlob
{
    public static class BlobService
    {
        private static CloudBlobContainer container;
        private static readonly object locker = new object();
        private static IConfiguration config;


        public static void IConfigurationConfigure(IConfiguration _config)
        {
            config = _config;
        }
        public static CloudBlobContainer Container => SetupContainer();

        private static CloudBlobContainer SetupContainer()
        {
            lock (locker)
            {
                if (container == null)
                {
                    IConfiguration configuration = config;

                    var keyVaultProvider = new AzureKeyVaultDataProvider(config);
                    ;
                    var secretValue = Task.Run(() => keyVaultProvider.GetSecretAsync(config.GetRequiredSection(FunctionsConstants.L10nKeyVaultUri).Value,
                         "StorageAccountAccessKey")).Result;
                    //var secretValue = Task.Run(() => keyVaultProvider.GetSecretAsync(ConfigurationManager.AppSettings[FunctionsConstants.L10nKeyVaultUri],
                    //    "StorageAccountAccessKey")).Result;

                    var connectionStringTemplate = config.GetRequiredSection("StorageConnectionString").Value;
                    var connectionString = connectionStringTemplate.Replace("${AccountKey}", secretValue);

                    var storageAccount = CloudStorageAccount.Parse(connectionString);

                    var blobClient = storageAccount.CreateCloudBlobClient();
                    container = blobClient.GetContainerReference(config.GetRequiredSection("StorageContainer").Value);
                    container?.CreateIfNotExists(BlobContainerPublicAccessType.Off);
                }
            }

            return container;
        }

        public static async Task<string> UploadFileAsync(string blobName, Stream inputStream)
        {
            DeleteIfExists(blobName);

            var blob = Container.GetBlockBlobReference(blobName);
            await blob.UploadFromStreamAsync(inputStream);

            return blobName;
        }

        private static void DeleteIfExists(string blobName)
        {
            var blob = Container.GetBlockBlobReference(blobName);
            blob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
        }

        public static async Task<DownloadAssetFileResult> DownloadFile(string blobName, string blobUri)
        {
            var blob = Container.GetBlockBlobReference(blobName);

            var stream = await blob.OpenReadAsync();
            return new DownloadAssetFileResult
            {
                FileContent = stream,
                FileName = blobName
            };
        }
    }
}