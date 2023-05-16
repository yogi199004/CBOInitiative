using System.Net;

namespace AAPS.L10nPortal.Bal.Exceptions
{
    public class ApplicationDeniedException : CustomHttpException
    {
        public ApplicationDeniedException() : base("You are not authorized to work with this Application or item was not found.", HttpStatusCode.BadRequest)
        {
        }
    }
}
