using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace AAPS.L10nPortal.Web.Services
{
    internal static class CustomResponseHelper
    {
        internal static HttpResponseMessage CreateErrorResponse(this HttpRequestMessage request, bool legacy, HttpStatusCode statusCode, string message, object data = null)
        {
            var response = new
            {
                error = new
                {
                    message,
                    data
                }
            };

            if (legacy)
            {
                return request.CreateCustomResponseMessage(response, "text/plain");
            }
            return request.CreateCustomResponseMessage(response, "application/json", statusCode);
        }

        internal static HttpResponseMessage CreateCustomResponseMessage(this HttpRequestMessage request, object obj, string mediaType, HttpStatusCode code = HttpStatusCode.OK)
        {
            HttpResponseMessage tempResponse = null;
            HttpResponseMessage response;

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 1
            };
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            try
            {
                tempResponse = request.CreateResponse(code);
                tempResponse.Content = new StringContent(JsonConvert.SerializeObject(obj, settings));
                tempResponse.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

                response = tempResponse;
                tempResponse = null;
            }
            finally
            {
                if (tempResponse != null)
                {
                    tempResponse.Dispose();
                }
            }

            return response;
        }
    }
}