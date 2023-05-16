using L10N.API.Common;
using L10N.API.Contracts;
using Microsoft.Azure.Cosmos;

namespace L10N.API.BAL
{
    public class CosmosService : ICosmosService
    {

        private static readonly CosmosClient? cosmosClient;
        private static Dictionary<string, string> cosmosConnectionProperties;
        static CosmosService()
        {
            cosmosClient = SetCosmosConnectionProperties().Result;
        }



        public static async Task<CosmosClient> SetCosmosConnectionProperties()
        {
            try
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
                return await CosmosClient.CreateAndInitializeAsync(cosmosConnectionProperties["EndPointUri"], cosmosConnectionProperties["PrimaryKeyReadOnly"], containers, options);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public CosmosClient GetConnection()
        {
            return cosmosClient;
        }

        public Container getContainer(CosmosClient cosmosClient, string appName)
        {
            Container con;
            try
            {
                Database db = cosmosClient.GetDatabase(cosmosConnectionProperties["DatabaseId"]);
                if (appName.ToLower().Contains("omnia"))
                {
                    con = db.GetContainer(cosmosConnectionProperties["OmniaContainerId"]);
                    return con;
                }
                else if (appName.ToLower().Contains("levvia"))
                {
                    con = db.GetContainer(cosmosConnectionProperties["LevviaContainerId"]);


                }
                else
                {
                    con = db.GetContainer(cosmosConnectionProperties["GeneralAppContainerId"]);

                }

                return con;

            }

            catch (Exception ex)
            {
                throw ex;
                //return null;
            }
        }
    }
}
