using CAPPortal.Contracts.Providers;
using CAPPortal.Common;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;

namespace CAPPortal.Secrets
{
    public class AzureKeyVaultDataProvider : IAzureKeyVaultDataProvider
    {
        private readonly string _connectionString;
        private readonly IConfiguration configuration;

        public AzureKeyVaultDataProvider(IConfiguration _configuration)
        {
            configuration = _configuration;
            _connectionString = configuration.GetRequiredSection(FunctionsConstants.AzureServiceConnectionString).Value;

        }

        public async Task<string> GetSecretAsync(string keyVaultUri, string secretName)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider(_connectionString);

            var keyVaultClient =
                new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            var secret = await keyVaultClient.GetSecretAsync(keyVaultUri, secretName);

            return secret.Value;
        }
    }
}
