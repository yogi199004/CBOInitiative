using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using L10N.API.BAL;
using L10N.API.Common;
using L10N.API.Contracts;
using L10N.API.Secrets;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(AAPS.L10nPortal.FunctionAPI.Startup))]


namespace AAPS.L10nPortal.FunctionAPI
{
    public class Startup : FunctionsStartup
    {
        //private ILoggerFactory _loggerFactory;
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddLogging();
            ConfigureServices(builder);
        }

        public void ConfigureServices(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IAzureKeyVaultDataProvider>((s) => { return new AzureKeyVaultDataProvider(); });
            // builder.Services.AddSingleton<ICosmosService>((s) => { return new CosmosService(); });
            builder.Services.AddSingleton<IAPIService>((s) => { return new APIService(); });

#if LOCAL
            ConfigValues.CosmosDbServerURI = Environment.GetEnvironmentVariable("Server");
            ConfigValues.PrimaryReadOnlyKey = Environment.GetEnvironmentVariable("PrimaryKey");
#else

            string clientid = Environment.GetEnvironmentVariable("UserAssignedIdentity");
            ConfigValues.KeyVaultURI = Environment.GetEnvironmentVariable("keyVaultUri");
            var client = new SecretClient(new Uri(ConfigValues.KeyVaultURI), new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = clientid }));
            var cosmosURI = client.GetSecret("CosmosServerURI");
            var primaryReadOnlyKey = client.GetSecret("PrimaryKey");
            var SAKey = client.GetSecret("StorageAccountAccessKeyv1");
            ConfigValues.CosmosDbServerURI = cosmosURI.Value.Value;
            ConfigValues.PrimaryReadOnlyKey = primaryReadOnlyKey.Value.Value;
            ConfigValues.SAKey = SAKey.Value.Value;
#endif
            ConfigValues.DatabaseID = Environment.GetEnvironmentVariable("Databaseid");
            ConfigValues.OmniaContainerId = Environment.GetEnvironmentVariable("OmniaContainerId");
            ConfigValues.LevviaContainerId = Environment.GetEnvironmentVariable("LevviaContainerId");
            ConfigValues.GeneralAppsContainerId = Environment.GetEnvironmentVariable("GeneralAppContainerId");
            ConfigValues.ApplicationName = Environment.GetEnvironmentVariable("ApplicationName");
            ConfigValues.CosmosPreferredRegion = Environment.GetEnvironmentVariable("Regions");
            ConfigValues.CosmosOtherRegions = Environment.GetEnvironmentVariable("OtherRegions");
            ConfigValues.StorageAccountConnectionString = Environment.GetEnvironmentVariable("StorageAccountConnectionString");
            ConfigValues.ContainerName = Environment.GetEnvironmentVariable("StorageContainer");
            //ConfigValues.SAKey = Environment.GetEnvironmentVariable("StorageAccountAccessKey");

        }
    }
}