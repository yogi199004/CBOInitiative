using L10N.API.Common;
using L10N.API.Contracts;
using L10N.API.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AAPS.L10nPortal.FunctionAPI
{
    public class GetMetadata
    {
        private readonly IAPIService apiService;
        public GetMetadata(IAPIService _apiService)
        {
            apiService = _apiService;
        }

        [FunctionName("GetMetadata")]
        public async Task<HttpResponseMessage> Run(
                [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetMetadata/{appName}")] HttpRequestMessage req,
                ILogger log, string appName)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(appName))
                {
                    return FunctionHelper.InvalidInputResposne();
                }
                else
                {
                    List<IEnumerable<MetaDataModel>> metaData;
                    var _cosmosContainerClient = CosmosServiceCommon.getContainer(CosmosServiceCommon.cosmosClient, appName);
                    metaData = await apiService.getAppMetaData(_cosmosContainerClient, appName);
                    if (metaData.Count() < 1)
                    {
                        return FunctionHelper.NoMetadataResponse(appName);
                    }
                    else
                    {
                        return new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(metaData.ToArray()[0]), Encoding.UTF8, "application/json")
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                log.LogError(ex, $" AppCode: {appName}; MethodName : {ex.GetType()};Error ID:{errorId};Error message:{ex.Message}");
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator. Error ID: " + errorId)
                };
                return response;
            }
        }






    }
}
