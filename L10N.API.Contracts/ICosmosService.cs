using Microsoft.Azure.Cosmos;

namespace L10N.API.Contracts
{
    public interface ICosmosService
    {
        CosmosClient GetConnection();
        Container getContainer(CosmosClient cosmosClient, string appName);

    }
}
