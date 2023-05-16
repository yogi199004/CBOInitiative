using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace L10n.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {

        private readonly ILogger<BaseController> log;

        public BaseController(ILogger<BaseController> _log)
        {
            log = _log;
        }
        protected Guid HandleException(Exception ex, string appcode, string methodName)
        {
            var errorId = Guid.NewGuid();

            log.LogError(ex, $" AppCode: {appcode}; MethodName : {methodName}; Error ID: {errorId}; Error message:{ex.Message}");

            return errorId;
        }


        protected HttpResponseMessage GetInternalServerErrorResponse(Guid errorId)
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(
                    $"{{ \"Message\" : \"An error occurred, please try again or contact the administrator. Error ID: {errorId}\"}}"),

                ReasonPhrase = "Critical Exception"
            };
        }
    }
}
