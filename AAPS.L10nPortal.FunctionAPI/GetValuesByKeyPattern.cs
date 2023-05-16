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
    public class GetValuesByKeyPattern
    {
        private readonly IAPIService apiService;
        public GetValuesByKeyPattern(IAPIService _apiService)
        {
            apiService = _apiService;
        }

        [FunctionName("GetValuesByKeyPattern")]
        public async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function,
            "post",
            Route = null)]HttpRequestMessage req, ILogger log)
        {
            // log.LogInformation("C# HTTP trigger function processed a request - GetValuesByKeyPattern");

            try
            {
                KeyModel data = JsonConvert.DeserializeObject<KeyModel>(req.Content.ReadAsStringAsync().Result);

                if (string.IsNullOrWhiteSpace(data.AppName) || string.IsNullOrWhiteSpace(data.Locale) || data.KeysPattern == null || data.KeysPattern.Length == 0)
                {
                    return FunctionHelper.InvalidInputResposne();
                }
                else
                {
                    Dictionary<string, string> result = new Dictionary<string, string>();
                    Dictionary<string, string> allKeyValues = new Dictionary<string, string>();

                    List<IEnumerable<MetaDataModel>> metaData;

                    var _cosmosClient = CosmosServiceCommon.cosmosClient; //logic to establish connection with cosmos
                    var _cosmosContainerClient = CosmosServiceCommon.getContainer(_cosmosClient, data.AppName);
                    metaData = await apiService.getAppMetaData(_cosmosContainerClient, data.AppName);

                    String currentLocale = string.Empty;

                    if (metaData.Count() < 1)
                    {
                        return FunctionHelper.NoMetadataResponse(data.AppName);
                    }
                    else
                    {
                        metaData.ForEach(x => { currentLocale = (x.Where(m => m.LocaleCode.Equals(data.Locale, StringComparison.OrdinalIgnoreCase))?.FirstOrDefault()?.LocaleCode?.ToString() ?? null); });

                        if (currentLocale != null)
                        {
                            List<KeyValues> keyValuesData = new List<KeyValues>();
                            keyValuesData = await apiService.getAppKeyValuesData(_cosmosContainerClient, data.AppName, data.Locale);

                            if (keyValuesData.ToArray().Length < 1)
                            {
                                return FunctionHelper.EmptyLocaleResponse(data.Locale);
                            }

                            else
                            {
                                allKeyValues = keyValuesData.ToArray()[0].AllKeyValues.keyValues;
                            }

                            foreach (string s in data.KeysPattern)
                            {
                                foreach (string key in allKeyValues.Keys)
                                {
                                    if ((!result.ContainsKey(key)) && key.StartsWith(s.Replace("*", String.Empty)))
                                    {
                                        result.Add(key, allKeyValues.GetValueOrDefault(key));
                                    }
                                }
                            }

                            if (result.Count == 0)
                            {
                                return FunctionHelper.EmptyLocaleResponse(data.Locale);
                            }
                            else
                            {
                                return new HttpResponseMessage(HttpStatusCode.OK)
                                {
                                    Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json")
                                };
                            }
                        }
                        else
                        {
                            return FunctionHelper.InvalidLocaleResponse(data.Locale);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                log.LogError(ex, $"  MethodName : {ex.GetType()};Error message:{ex.Message};Error ID:{errorId}");
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator. Error ID: " + errorId)
                };
                return response;
            }

        }
    }
}
