using System.Net;

namespace CAPPortal.Bal.Exceptions
{
    public class BadRequestException : CustomHttpException
    {
        public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
        {
        }
    }
}
