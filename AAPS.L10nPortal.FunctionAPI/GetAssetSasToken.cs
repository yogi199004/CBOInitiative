using L10N.API.Common;
using L10N.API.Contracts;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AAPS.L10nPortal.FunctionAPI
{
    public class GetAssetSasToken
    {
        private readonly IAPIService apiService;
        public GetAssetSasToken(IAPIService _apiService)
        {
            apiService = _apiService;
        }

        [FunctionName("GetAssetSasToken")]
        public async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function,
            "post",
            Route = null)]HttpRequestMessage req, ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            var AssetName = await req.Content.ReadAsStringAsync();
            try
            {


                if (string.IsNullOrEmpty(AssetName.ToString()))
                {
                    return null;
                }
                else
                {
                    var assetUrlWithSas = apiService.GetSASUrl(AssetName.ToString(), ConfigValues.SAKey);
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(assetUrlWithSas, Encoding.UTF8, "text/plain")
                    };
                }
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                log.LogError(ex, $" MethodName : {ex.GetType()};Error ID:{errorId};Error message:{ex.Message}");
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator. Error ID: " + errorId)
                };
                return response;
            }

        }
    }
}
