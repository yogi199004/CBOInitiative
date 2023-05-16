using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;


namespace AAPS.L10nPortal.Dal
{
    public abstract class AzureApiDataProviderBase
    {
        protected Uri BaseUri;

        protected HttpClient HttpClient { get; }

        protected AzureApiDataProviderBase()
        {
            HttpClient = new HttpClient();
        }

        protected Task<T> CallPostApiMethodAsync<T>(string queryPath, object body, string accessToken,
                          CancellationToken cancellationToken)
        {
            return CallApiMethodAsync<T>(queryPath, HttpMethod.Post, body, accessToken, cancellationToken);
        }

        protected async Task<T> CallApiMethodAsync<T>(string queryPath, HttpMethod httpMethod,
                                object body, string accessToken, CancellationToken cancellationToken)
        {
            async Task<T> MessageHandler(HttpRequestMessage requestMessage, string appParamName)
            {
                requestMessage.Method = httpMethod;

                if (httpMethod == HttpMethod.Post || httpMethod == new HttpMethod("PATCH"))
                {
                    var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8,
                    "application/json");
                    requestMessage.Content = content;
                }

                requestMessage.Headers.Accept.Clear();
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var responseMessage = await HttpClient.SendAsync(requestMessage, cancellationToken))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        var content = await responseMessage.Content.ReadAsStringAsync();
                        throw new Exception($"Status code: {responseMessage.StatusCode}; Message: {content}");
                    }

                    var result = await responseMessage.Content.ReadAsAsync<T>(cancellationToken);

                    return result;
                }
            }

            return await SendHttpMessageAsync(queryPath, httpMethod, accessToken, MessageHandler);
        }

        private async Task<T> SendHttpMessageAsync<T>(string queryPath, HttpMethod httpMethod,
                              string accessToken, Func<HttpRequestMessage, string, Task<T>> messageHandler)
        {
            var uri = new Uri(BaseUri.AbsoluteUri + queryPath);

            using (var requestMessage = new HttpRequestMessage(httpMethod, uri))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                return await messageHandler(requestMessage, GetClientName());
            }
        }

        protected abstract string GetClientName();
    }
}
