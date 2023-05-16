using L10N.API.Common;
using L10N.API.Contracts;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace L10N.API.Secrets
{
    public class AzureKeyVaultDataProvider : IAzureKeyVaultDataProvider
    {

        public async Task<string> GetSecretAsync(string keyVaultUri, string secretName)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider(ConfigValues.AzureServiceConnectionString);

            var keyVaultClient =
                new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            var secret = await keyVaultClient.GetSecretAsync(keyVaultUri, secretName);

            return secret.Value;
        }

    }
}
