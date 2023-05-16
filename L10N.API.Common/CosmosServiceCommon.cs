using Microsoft.Azure.Cosmos;

namespace L10N.API.Common
{
    public static class CosmosServiceCommon
    {
        private static Lazy<CosmosClient> lazyClient = new Lazy<CosmosClient>(InitializeCosmosClient);
        public static CosmosClient cosmosClient => lazyClient.Value;

        private static Dictionary<string, string> cosmosConnectionProperties;
        private static CosmosClient InitializeCosmosClient()
        {
            List<string> cosmosRegion = new List<string>();
            cosmosRegion.Add(ConfigValues.CosmosPreferredRegion);
            foreach (string s in ConfigValues.CosmosOtherRegions.Split(','))
            {
                cosmosRegion.Add(s);
            }

            var cosmosConnectionProperties = GetCosmosConnection();


            List<(string, string)> containers = new List<(string, string)>
            {
              (cosmosConnectionProperties["DatabaseId"], cosmosConnectionProperties["OmniaContainerId"]),
              (cosmosConnectionProperties["DatabaseId"], cosmosConnectionProperties["LevviaContainerId"]),
              (cosmosConnectionProperties["DatabaseId"], cosmosConnectionProperties["GeneralAppContainerId"])
            };

            CosmosClientOptions options = new CosmosClientOptions();
            options.ApplicationName = ConfigValues.ApplicationName;
            options.ApplicationPreferredRegions = cosmosRegion;
            options.ConnectionMode = ConnectionMode.Direct;
            return CosmosClient.CreateAndInitializeAsync(cosmosConnectionProperties["EndPointUri"], cosmosConnectionProperties["PrimaryKeyReadOnly"], containers, options).Result;


        }

        private static Dictionary<string, string> GetCosmosConnection()
        {
            cosmosConnectionProperties = new Dictionary<string, string>();
            cosmosConnectionProperties.Add("DatabaseId", ConfigValues.DatabaseID);
            cosmosConnectionProperties.Add("OmniaContainerId", ConfigValues.OmniaContainerId);
            cosmosConnectionProperties.Add("LevviaContainerId", ConfigValues.LevviaContainerId);
            cosmosConnectionProperties.Add("GeneralAppContainerId", ConfigValues.GeneralAppsContainerId);
            cosmosConnectionProperties.Add("EndPointUri", ConfigValues.CosmosDbServerURI);
            cosmosConnectionProperties.Add("PrimaryKeyReadOnly", ConfigValues.PrimaryReadOnlyKey);

            return cosmosConnectionProperties;
        }


        public static Container getContainer(CosmosClient cosmosClient, string appName)
        {
            Container con;
            try
            {
                Database db = cosmosClient.GetDatabase(ConfigValues.DatabaseID);
                if (appName.ToLower().Contains("omnia"))
                {
                    con = db.GetContainer(ConfigValues.OmniaContainerId);
                    return con;
                }
                else if (appName.ToLower().Contains("levvia"))
                {
                    con = db.GetContainer(ConfigValues.LevviaContainerId);


                }
                else
                {
                    con = db.GetContainer(ConfigValues.GeneralAppsContainerId);

                }

                return con;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
