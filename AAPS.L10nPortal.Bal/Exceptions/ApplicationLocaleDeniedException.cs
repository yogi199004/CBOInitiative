using System.Net;

namespace AAPS.L10nPortal.Bal.Exceptions
{
    public class ApplicationLocaleDeniedException : CustomHttpException
    {
        public ApplicationLocaleDeniedException() : base("You are not authorized to work with this Application Locale or item was not found.", HttpStatusCode.BadRequest)
        {
        }
    }
}
