using L10N.API.Common;
using L10N.API.SyncFunction.DAL;
using L10N.API.SyncFunction.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace L10N.API.SyncFunction.BAL
{
    public class FunctionService
    {
        private CosmosClient cosmosClient;

        private Database database;

        private Container container;

        private string databaseId;

        private string endPointURI;

        private string primaryKey;

        private string containerIds;

        private static string connectionString;

        private string omniaContainerId;

        private string levviaContainerId;

        private string generalAppContainerId;

        private ILogger _log;
        FunctionDataProvider fdp = null;

        public FunctionService(ILogger log, CosmosClient _cosmosClient)
        {
            _log = log;
            cosmosClient = _cosmosClient;
        }

        public async Task SyncDataToCosmos()
        {
            try
            {
                //SetUpCosmosConnection();
                //Will create a Cosmos DB Database
                await this.CreateDatabaseAsync();

                //Will create container inside the DB
                await this.CreateContainerAsync();

                //Willadd items to the container
                await this.AddDataToContainerAsync();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
            }

        }

        private CosmosClient SetUpCosmosConnection()
        {

            string keyVaultURI;

            endPointURI = System.Environment.GetEnvironmentVariable("Server");
            primaryKey = System.Environment.GetEnvironmentVariable("PrimaryKey");
            databaseId = System.Environment.GetEnvironmentVariable("Databaseid");


            if (endPointURI is null || primaryKey is null)
            {
                _log.LogError("Cosmos Connection properties missing");
                throw new Exception("Cosmos Connection properties missing");
            }

            this.cosmosClient = new CosmosClient(endPointURI, primaryKey, new CosmosClientOptions()
            {
                ConnectionMode = ConnectionMode.Direct
            });

            return this.cosmosClient;
        }

        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(ConfigValues.DatabaseID);

            _log.LogInformation("Create Database Operation Completed at {0}", DateTime.Now.ToString());

        }

        private async Task CreateContainerAsync()
        {
            containerIds = ConfigValues.OmniaContainerId + ',' + ConfigValues.LevviaContainerId + ',' + ConfigValues.GeneralAppsContainerId;
            string[] containerarrray = containerIds.Split(',');
            foreach (string s in containerarrray)
            {
                // Create a new container
                this.container = await this.database.CreateContainerIfNotExistsAsync(s, "/AppName");

            }

            _log.LogInformation("Create Container Operation Completed at {0}", DateTime.Now.ToString());
        }

        private async Task AddDataToContainerAsync()
        {
            List<Application> lstApplication = GetL10NApps();//Get all apps from DB
            List<Application> lstOmniaApps = lstApplication.Where(app => app.Name.ToLower().Contains("omnia")).ToList();
            List<Application> lstLevviaApps = lstApplication.Where(app => app.Name.ToLower().Contains("levvia")).ToList();
            List<Application> lstGeneralApps = lstApplication.Except(lstOmniaApps).Except(lstLevviaApps).ToList();

            //Get Apps Data

            List<MetaDataModel> appsMetaData = await GetAppsMetaData();
            List<App10NKeysandValuescs> AppsKeyValues = await GetL10NKeyValues();
            List<Application> deletedApps = GetDeletedLocales();
            List<Application> appsWithDeletedKeys = GetAppsWithDeletedKeys();

            //sync data for Omnia Container

            await SyncAppsDataToCosmos(lstOmniaApps, appsMetaData, AppsKeyValues, "Omnia", deletedApps, appsWithDeletedKeys);
            _log.LogInformation("Sync completed for Omnia apps");

            //sync data for Levvia Container
            await SyncAppsDataToCosmos(lstLevviaApps, appsMetaData, AppsKeyValues, "Levvia", deletedApps, appsWithDeletedKeys);
            _log.LogInformation("Sync completed for Levvia apps");

            //sync data for General Container
            await SyncAppsDataToCosmos(lstGeneralApps, appsMetaData, AppsKeyValues, "Others", deletedApps, appsWithDeletedKeys);
            _log.LogInformation("Sync completed for general apps");
        }


        private async Task SyncAppsDataToCosmos(List<Application> lstApplication, List<MetaDataModel> appsMetaData, List<App10NKeysandValuescs> AppsKeyValues, string container, List<Application> lstDeletedAppLocale, List<Application> lstAppsWithDeletedKeys)
        {
            Container Container = getContainer(cosmosClient, container);


            foreach (Application app in lstApplication)
            {
                if (appsMetaData != null)
                {
                    //List<MetaDataModel> containerSpecificMetadata = appsMetaData.Where(app => app.AppName.ex (container)).ToList();
                    _log.LogInformation("Get App sepecific metadata operation started for {0} at {1}", app.Name, DateTime.Now.ToString());
                    List<MetaDataModel> appSpecificMetaData = GetAppSpecificMetaData(app.Name, appsMetaData);
                    _log.LogInformation("Get App sepecific metadata operation completed for {0} at {1}", app.Name, DateTime.Now.ToString());
                    if (appSpecificMetaData.Count > 0)
                    {
                        _log.LogInformation("Add App sepecific metadata operation started for {0} at {1}", appSpecificMetaData.FirstOrDefault().AppName, DateTime.Now.ToString());
                        await AddAppMetaDataToCosmos(Container, appSpecificMetaData);
                        _log.LogInformation("Add App sepecific metadata operation completed for {0} at {1}", appSpecificMetaData.FirstOrDefault().AppName, DateTime.Now.ToString());
                    }
                }
                if (AppsKeyValues != null)
                {
                    //List<App10NKeysandValuescs> containerSpecificAppKeyValue = AppsKeyValues.Where(app => app.AppName.Contains(container)).ToList();
                    _log.LogInformation("Add App sepecific key values operation started at {0} for App {1}", DateTime.Now.ToString(), app.Name);
                    List<App10NKeysandValuescs> appSpecificKeyValues = GetAppSpecificKeyValues(app.Name, AppsKeyValues);

                    if (appSpecificKeyValues.Count > 0)
                    {
                        await AddAppKeyValueDataToCosmos(Container, appSpecificKeyValues);
                        _log.LogInformation("Add App sepecific key values operation completed at {0} for App {1}", DateTime.Now.ToString(), app.Name);
                    }


                }
                if (lstDeletedAppLocale.Count > 0)
                {
                    var DeletedApp = lstDeletedAppLocale.Where(x => x.Name.Equals(app.Name)).FirstOrDefault<Application>();
                    if (DeletedApp != null)
                    {
                        string AppName = (DeletedApp.Name + "_" + DeletedApp.Locale).ToLower();
                        await DeleteLocaleSpecificContent(Container, AppName);
                    }
                }
                if (lstAppsWithDeletedKeys.Count > 0)
                {
                    var appWithDeletedKey = lstAppsWithDeletedKeys.Where(x => x.Name.Equals(app.Name)).FirstOrDefault<Application>();
                    if (appWithDeletedKey != null)
                    {
                        //Get Deleted App Key values
                        List<App10NKeysandValuescs> KeyValuesForAppsWithDeletedKeys = await GetL10NKeyValuesForDeletedApps(app.Name);

                        //Sync Deleted App Key values
                        if (KeyValuesForAppsWithDeletedKeys.Count > 0)
                        {
                            await AddAppKeyValueDataToCosmos(Container, KeyValuesForAppsWithDeletedKeys);
                            _log.LogInformation("Add App sepecific key values operation completed at {0} for App {1}", DateTime.Now.ToString(), app.Name);
                        }

                    }
                }
            }
        }
        public List<Application> GetL10NApps()
        {
            fdp = new FunctionDataProvider();
            connectionString = GetConnectionString();
            _log.LogInformation("The Value of connection string is {0}", connectionString);
            _log.LogInformation("Get L10N App List operation Completed at {0}", DateTime.Now.ToString());
            return fdp.GetL10NApps(connectionString);


        }


        private string GetConnectionString()
        {
            string dbConnectionString;
            string PasswordPattern = "$(Password)";
            string secretValue;
            string keyVaultURI;


            connectionString = System.Environment.GetEnvironmentVariable("L10nPortal");

            //keyVaultURI = ConfigValues.KeyVaultURI;
            //secretValue = Task.Run(() => AzureKeyVaultDataProvider.GetSecretAsync(keyVaultURI, "L10nServerPassword")).Result;
            //var connectionStringTemplate = System.Environment.GetEnvironmentVariable("L10nPortal");
            //connectionString = connectionStringTemplate.Replace(PasswordPattern, secretValue);

            return connectionString;
        }

        public List<MetaDataModel> GetAppSpecificMetaData(string appName, List<MetaDataModel> AppsMetaData)
        {
            return AppsMetaData.Where(x => x.AppName == appName).ToList();

        }



        public List<App10NKeysandValuescs> GetAppSpecificKeyValues(string appName, List<App10NKeysandValuescs> appKeyValues)
        {
            return appKeyValues.Where(x => x.AppName == appName).ToList();
        }

        public async Task<List<MetaDataModel>> GetAppsMetaData()
        {
            fdp = new FunctionDataProvider();

            return await fdp.GetAppsMetaData(connectionString);
        }


        public void UpdateSyncConfiguration()
        {
            try
            {
                fdp = new FunctionDataProvider();
                fdp.UpdateSyncConfiguration(connectionString);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
            }

        }


        public async Task AddAppMetaDataToCosmos(Container container, List<MetaDataModel> appSpecificMetaData)
        {


            string appName = (appSpecificMetaData.FirstOrDefault().AppName + "_MetaData").ToLower();

            _log.LogInformation("Delete operation for app {0} ", appName);

            AppMetaData app = container.GetItemLinqQueryable<AppMetaData>(true)
                     .Where(m => m.AppName == appName)
                     .AsEnumerable()
                     .FirstOrDefault();
            if (app != null)
            {
                _log.LogInformation("Delete Metadata from cosmos initiated for app {0} with partiion key as {1} and id as {2}", appName, app.AppName, app.id.ToString());
                CosmosItemDeletionResponseModel response = await container.DeleteItemAsync<CosmosItemDeletionResponseModel>(app.id.ToString(), new PartitionKey(app.AppName));

            }

            AppMetaData appMetaData = new AppMetaData();

            appMetaData.MetaData = appSpecificMetaData;
            appMetaData.AppName = appName;

            ItemResponse<AppMetaData> appContentResponse = await container.CreateItemAsync<AppMetaData>(appMetaData, new PartitionKey(appMetaData.AppName));



        }

        public Container getContainer(CosmosClient cosmosClient, string appName)
        {
            try
            {
                Database db = cosmosClient.GetDatabase(ConfigValues.DatabaseID);
                if (appName.ToLower().Contains("omnia"))
                {
                    Container con = db.GetContainer(ConfigValues.OmniaContainerId);
                    return con;
                }
                else if (appName.ToLower().Contains("levvia"))
                {
                    Container con = db.GetContainer(ConfigValues.LevviaContainerId);
                    return con;
                }
                else
                {
                    Container con = db.GetContainer(ConfigValues.GeneralAppsContainerId);
                    return con;
                }

            }

            catch (Exception ex)
            {
                return null;
            }
        }



        public async Task AddAppKeyValueDataToCosmos(Container container, List<App10NKeysandValuescs> appKeyValues)
        {
            if (appKeyValues.Count >= 1)
            {
                string appName = appKeyValues.FirstOrDefault().AppName;
                List<string> distinctLocales = appKeyValues.Select(x => x.LocaleCode).Distinct().ToList();

                foreach (string locale in distinctLocales)
                {
                    _log.LogInformation("Add App sepecific key values to Cosmos operation started for App {0} for Locale {1} at {2}", appName, locale, DateTime.Now.ToString());
                    App10NKeysandValuescs LocalekeyValues = new App10NKeysandValuescs();
                    LocalekeyValues.AllKeyValues = new AllKeyValues();
                    LocalekeyValues.AllKeyValues.keyValues = new Dictionary<string, string>();

                    List<App10NKeysandValuescs> LocaleSpecificKeyValues = appKeyValues.Where(x => x.LocaleCode == locale).ToList();
                    if (LocaleSpecificKeyValues.Count >= 1)
                    {
                        _log.LogInformation("Inside  if condition for App sepecific key values to Cosmos for App {0} for Locale {1} ", appName, locale);
                        LocalekeyValues.LocaleCode = LocaleSpecificKeyValues.FirstOrDefault().LocaleCode;
                        LocalekeyValues.AppName = (LocaleSpecificKeyValues.FirstOrDefault().AppName + "_" + LocaleSpecificKeyValues.FirstOrDefault().LocaleCode).ToLower();
                        LocalekeyValues.CreatedDate = LocaleSpecificKeyValues.FirstOrDefault().CreatedDate;
                        LocalekeyValues.UpdatedDate = LocaleSpecificKeyValues.FirstOrDefault().UpdatedDate;
                        LocalekeyValues.CreatedBy = LocaleSpecificKeyValues.FirstOrDefault().CreatedBy;
                        LocalekeyValues.UpdatedBy = LocaleSpecificKeyValues.FirstOrDefault().UpdatedBy;

                        foreach (App10NKeysandValuescs kv in LocaleSpecificKeyValues)
                        {
                            LocalekeyValues.AllKeyValues.keyValues.Add(kv.ResourcKey, kv.LocaleValue);
                            //LocalekeyValues.keyValues.Add(kv.ResourcKey, kv.LocaleValue);
                        }


                        App10NKeysandValuescs LocaleSpecificContent = container.GetItemLinqQueryable<App10NKeysandValuescs>(true)
                       .Where(m => m.AppName == LocalekeyValues.AppName)
                       .AsEnumerable()
                       .FirstOrDefault();

                        if (LocaleSpecificContent != null)
                        {
                            ItemResponse<App10NKeysandValuescs> appContentResponseDelete = await container.DeleteItemAsync<App10NKeysandValuescs>(LocaleSpecificContent.id.ToString(), new PartitionKey(LocalekeyValues.AppName));
                        }



                        ItemResponse<App10NKeysandValuescs> appContentResponseCreate = await container.CreateItemAsync<App10NKeysandValuescs>(LocalekeyValues, new PartitionKey(LocalekeyValues.AppName));
                        if (appContentResponseCreate.StatusCode == System.Net.HttpStatusCode.Created)

                        {
                            _log.LogInformation("Add App sepecific key values to Cosmos operation completed for App {0} for Locale {1} with partition key as {2}", appName, locale, LocalekeyValues.AppName);
                        }

                    }


                }
            }

        }
        public Task<List<App10NKeysandValuescs>> GetL10NKeyValues()
        {
            fdp = new FunctionDataProvider();
            return fdp.GetAppsKeyValues(connectionString);
        }

        public Task<List<App10NKeysandValuescs>> GetL10NKeyValuesForDeletedApps(string appName)
        {
            fdp = new FunctionDataProvider();
            return fdp.GetAppsKeyValuesForDeletedApps(connectionString, appName);
        }

        public List<Application> GetDeletedLocales()
        {
            fdp = new FunctionDataProvider();
            connectionString = GetConnectionString();
            _log.LogInformation("Get L10N App List operation Completed at {0}", DateTime.Now.ToString());
            return fdp.GetDeletedLocales(connectionString);

        }

        public List<Application> GetAppsWithDeletedKeys()
        {
            fdp = new FunctionDataProvider();
            connectionString = GetConnectionString();
            _log.LogInformation("Get L10N App List operation Completed at {0}", DateTime.Now.ToString());
            return fdp.GetAppsWithDeletedKeys(connectionString);

        }


        public async Task DeleteLocaleSpecificContent(Container container, string AppName)
        {
            App10NKeysandValuescs LocaleSpecificContent = container.GetItemLinqQueryable<App10NKeysandValuescs>(true)
                       .Where(m => m.AppName == AppName)
                       .AsEnumerable()
                       .FirstOrDefault();

            if (LocaleSpecificContent != null)
            {
                ItemResponse<App10NKeysandValuescs> appContentResponseDelete = await container.DeleteItemAsync<App10NKeysandValuescs>(LocaleSpecificContent.id.ToString(), new PartitionKey(AppName));
            }
        }
    }
}
