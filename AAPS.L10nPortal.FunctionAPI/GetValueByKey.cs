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
    public class GetValueByKey
    {
        private readonly IAPIService apiService;
        public GetValueByKey(IAPIService _apiService)
        {
            apiService = _apiService;
        }

        [FunctionName("GetValueByKey")]
        public async Task<HttpResponseMessage> Run(
                [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetValueByKey/{appName}/{locale}/{key}")]
                HttpRequestMessage req, ILogger log,
                string appName,
                string locale,
                string key
            )
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                List<IEnumerable<MetaDataModel>> metaData;
                if (string.IsNullOrWhiteSpace(appName) || string.IsNullOrWhiteSpace(locale) || string.IsNullOrWhiteSpace(key))
                {
                    return FunctionHelper.InvalidInputResposne();
                }
                else
                {
                    var _cosmosClient = CosmosServiceCommon.cosmosClient;
                    var _cosmosContainerClient = CosmosServiceCommon.getContainer(_cosmosClient, appName);
                    metaData = await apiService.getAppMetaData(_cosmosContainerClient, appName);

                    string currentLocale = string.Empty;

                    if (metaData.Count() < 1)
                    {
                        return FunctionHelper.NoMetadataResponse(appName);
                    }
                    else
                    {
                        metaData.ForEach(x => { currentLocale = (x.Where(m => m.LocaleCode.Equals(locale, StringComparison.OrdinalIgnoreCase))?.FirstOrDefault()?.LocaleCode?.ToString() ?? null); });

                        if (currentLocale != null)
                        {
                            List<KeyValues> keyValuesData = new List<KeyValues>();
                            keyValuesData = await apiService.getAppKeyValuesData(_cosmosContainerClient, appName, locale);

                            if (keyValuesData.ToArray().Length > 0 && keyValuesData.ToArray()[0].AllKeyValues.keyValues.ContainsKey(key))
                            {
                                return new HttpResponseMessage(HttpStatusCode.OK)
                                {
                                    Content = new StringContent(JsonConvert.SerializeObject(keyValuesData.ToArray()[0].AllKeyValues.keyValues[key]), Encoding.UTF8, "application/json")
                                };
                            }
                            else
                            {
                                return FunctionHelper.EmptyKeyOrValueReponse(key, locale);
                            }
                        }
                        else
                        {
                            return FunctionHelper.InvalidLocaleResponse(locale);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                log.LogError(ex, $" AppCode: {appName};Locale: {locale}; key : {key}; MethodName : {ex.GetType()};Error ID:{errorId};Error message:{ex.Message}");
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator. Error ID: " + errorId)
                };
                return response;
            }

        }
    }
}
