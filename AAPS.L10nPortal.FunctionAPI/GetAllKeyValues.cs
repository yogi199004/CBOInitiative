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
    public class GetAllKeyValues
    {
        private readonly IAPIService apiService;
        public GetAllKeyValues(IAPIService _apiService)
        {
            apiService = _apiService;
        }


        [FunctionName("GetAllKeyValues")]
        public async Task<HttpResponseMessage> Run(
               [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetAllKeyValues/{appName}/{locale}")]
                HttpRequestMessage req, ILogger log,
               string appName,
               string locale
           )
        {
            try
            {
                if (string.IsNullOrWhiteSpace(locale) || string.IsNullOrWhiteSpace(appName))
                {
                    return FunctionHelper.InvalidInputResposne();
                }
                else
                {
                    List<IEnumerable<MetaDataModel>> metaData;
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

                            if (keyValuesData.ToArray().Length < 1)
                            {
                                return FunctionHelper.EmptyLocaleResponse(locale);
                            }
                            else
                            {
                                return new HttpResponseMessage(HttpStatusCode.OK)
                                {
                                    Content = new StringContent(JsonConvert.SerializeObject(keyValuesData.ToArray()[0].AllKeyValues.keyValues), Encoding.UTF8, "application/json")
                                };
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
                log.LogError(ex, $" AppCode: {appName}; Locale: {locale}; MethodName : {ex.GetType()};Error ID:{errorId};Error message:{ex.Message}");
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator. Error ID: " + errorId)
                };
                return response;
            }
        }
    }
}
