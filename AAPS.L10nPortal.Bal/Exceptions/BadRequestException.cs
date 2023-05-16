using System.Net;

namespace AAPS.L10nPortal.Bal.Exceptions
{
    public class BadRequestException : CustomHttpException
    {
        public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
        {
        }
    }
}
