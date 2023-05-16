using AAPS.L10NPortal.Common;
using System.Net;

namespace L10N.API.Common
{
    public static class FunctionHelper
    {
        public static HttpResponseMessage InvalidLocaleResponse(string locale)
        {

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(String.Format(FunctionsConstants.InvalidLocale, locale))
            };
        }
        public static HttpResponseMessage NoMetadataResponse(string appName)
        {

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(String.Format(FunctionsConstants.NoMetadata, appName))
            };
        }
        public static HttpResponseMessage InvalidInputResposne()
        {

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(FunctionsConstants.InvalidInputs)
            };
        }
        public static HttpResponseMessage EmptyLocaleResponse(string locale)
        {

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(String.Format(FunctionsConstants.EmptyLocale, locale))
            };
        }
        public static HttpResponseMessage EmptyKeyOrValueReponse(string key, string locale)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(String.Format(FunctionsConstants.KeyOrValueNotExists, key, locale))
            };
        }
        public static HttpResponseMessage ExceptionMessage(string exceptinMessage)
        {

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(exceptinMessage)
            };
        }

    }
}
