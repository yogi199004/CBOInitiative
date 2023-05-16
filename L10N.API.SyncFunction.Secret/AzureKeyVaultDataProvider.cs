using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace L10N.API.SyncFunction.Secret
{
    public class AzureKeyVaultDataProvider
    {

        public static async Task<string> GetSecretAsync(string keyVaultUri, string secretName)
        {
            string azureServiceConnectionString = System.Environment.GetEnvironmentVariable("AzureServiceConnectionString");
            var azureServiceTokenProvider = new AzureServiceTokenProvider(azureServiceConnectionString);

            var keyVaultClient =
                new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            var secret = await keyVaultClient.GetSecretAsync(keyVaultUri, secretName);

            return secret.Value;
        }



    }
}
