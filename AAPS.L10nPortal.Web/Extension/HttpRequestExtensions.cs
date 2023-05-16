using Microsoft.AspNetCore.StaticFiles;
using System.Net;
using System.Net.Http.Headers;

namespace AAPS.L10nPortal.Web.Extension
{
    public static class HttpRequestExtensions
    {
        public static HttpResponseMessage BytesToHttpResponse(this byte[] bytes, string filename)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(bytes);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = filename;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(HttpRequestExtensions.GetMimeTypeForFileExtension(filename));
            return response;
        }

        public static HttpResponseMessage StreamToHttpResponse(this Stream stream, string filename)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = filename;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(HttpRequestExtensions.GetMimeTypeForFileExtension(filename));
            return response;
        }


        public static string GetMimeTypeForFileExtension(string filePath)
        {
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out string contentType))
            {
                contentType = DefaultContentType;
            }

            return contentType;
        }
    }
}
