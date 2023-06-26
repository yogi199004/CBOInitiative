using System.Net;

namespace AAPS.CAPPortal.Bal.Exceptions
{
    public class BadRequestException : CustomHttpException
    {
        public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
        {
        }
    }
}
