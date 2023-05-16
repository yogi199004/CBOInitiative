using Microsoft.Extensions.Logging;

namespace L10N.API.SyncFunction.Secret
{
    public class ConnectionStringProvider
    {
        private const string PasswordPattern = "$(Password)";
        public static async Task<string> GetConnectionString(string passwordSecretName, ILogger log)
        {

            try
            {
                string connectionString;

                //#if LOCAL
                //                log.LogInformation("Getting connection string from Config files");
                //                connectionString = System.Environment.GetEnvironmentVariable("L10nPortal");

                //#else
                log.LogInformation("Getting connection string from key vault");
                var keyVaultUri = System.Environment.GetEnvironmentVariable("keyVaultUri");
                var secretValue = await AzureKeyVaultDataProvider.GetSecretAsync(keyVaultUri, passwordSecretName);
                var connectionStringTemplate = System.Environment.GetEnvironmentVariable("L10nPortal");
                connectionString = connectionStringTemplate.Replace(PasswordPattern, secretValue);

                //#endif

                return connectionString;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
