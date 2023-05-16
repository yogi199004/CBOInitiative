using L10N.API.Common;
using L10N.API.SyncFunction.BAL;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace L10N.API.SyncFunction
{
    public class SyncKeyValues
    {
        //    private readonly ICosmosService cosmosService;
        //    public SyncKeyValues(ICosmosService _cosmosService)
        //    {
        //        cosmosService = _cosmosService;
        //    }

        [FunctionName("SyncKeyValues")]
        public async Task Run([TimerTrigger("%SyncSchedule%")] TimerInfo myTimer, ILogger log)
        {
            DateTime syncStartTime = DateTime.Now;
            log.LogInformation("Sync started at {0}", syncStartTime.ToString());
            var cosmosClient = CosmosServiceCommon.cosmosClient;
            FunctionService funcService = new FunctionService(log, cosmosClient);

            await funcService.SyncDataToCosmos();
            funcService.UpdateSyncConfiguration();
            log.LogInformation("Sync Execution time is {0}", (DateTime.Now - syncStartTime).ToString());
        }
    }
}
