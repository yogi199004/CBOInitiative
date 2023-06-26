using System.Net;

namespace AAPS.CAPPortal.Bal.Exceptions
{
    public class ApplicationDeniedException : CustomHttpException
    {
        public ApplicationDeniedException() : base("You are not authorized to work with this Application or item was not found.", HttpStatusCode.BadRequest)
        {
        }
    }
}
