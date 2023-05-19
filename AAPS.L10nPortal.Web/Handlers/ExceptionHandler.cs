
using AAPS.L10nPortal.Bal.Exceptions;
using AAPS.L10nPortal.Dal.Exceptions;
using AAPS.L10nPortal.Web.Models;
using System.Net;
using System.Net.Mime;

namespace AAPS.L10nPortal.Web.Handlers
{
    public class ExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {

                httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*.deloitte.com");
                httpContext.Response.Headers.Add("Content-Security-Policy", "default-src 'self' 'unsafe-inline' https://localhost:51974/ ; style-src 'self' 'unsafe-inline' 'unsafe-eval' https://localhost:51974/ ; font-src 'self' data:; img-src data: 'self' https://localhost:51974/ cdn.cookielaw.org; script-src 'self' 'unsafe-inline' 'unsafe-eval' https://localhost:51974/ https://js-agent.newrelic.com/ https://bam.nr-data.net/ cdn.cookielaw.org geolocation.onetrust.com; connect-src 'self' wss://localhost:44334/AAPS.L10nPortal.Web/");
                //httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                // httpContext.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
                httpContext.Response.Headers.Add("Referrer-Policy", "no-referrer");
                httpContext.Response.Headers.Add("Cache-Control", "no-store");
                httpContext.Response.Headers.Remove("server");


                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var error = HandleException(httpContext, ex);
                if (!httpContext.Response.HasStarted && error is not null)
                {

                    httpContext.Response.Clear();
                    httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                    httpContext.Response.StatusCode = (int)error.StatusCode;
                    await httpContext.Response.WriteAsJsonAsync<ErrorMessageResult>(error);
                }
                else
                {
                    httpContext.Response.Clear();
                    httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                    httpContext.Response.StatusCode = error is not null ? (int)error.StatusCode : 500;
                    await httpContext.Response.WriteAsJsonAsync<ErrorMessageResult>(error);
                }
            }
        }


        public ErrorMessageResult HandleException(HttpContext context, Exception exception)
        {


            var userInfo = "User Email : " + context.User.Identity.Name;

            _logger.LogError(exception, userInfo);

            return HandleCore(exception, context);

        }

        public ErrorMessageResult HandleCore(Exception exception, HttpContext context)
        {


            if (exception is CustomHttpException)
            {
                var customHttpException = (CustomHttpException)exception;
                var error = new ErrorMessageResult((int)customHttpException.StatusCode, exception.Message);
                return error;
            }
            else if (exception is PermissionException)
            {
                var error = new ErrorMessageResult((int)HttpStatusCode.Forbidden, exception.Message);
                return error;
            }
            else if (exception is ExcelCorruptedException)
            {
                var error = new ErrorMessageResult(500, exception.Message);
                return error;
            }
            else if (exception is CustomSqlException && !(exception is UndefinedException))
            {
                var error = new ErrorMessageResult((int)HttpStatusCode.BadRequest, exception.Message);
                return error;
            }


            else
            {
                return null;
            }


        }


    }
}