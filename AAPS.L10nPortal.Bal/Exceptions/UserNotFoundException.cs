using System.Net;

namespace AAPS.L10nPortal.Bal.Exceptions
{
    [Serializable]
    public class UserNotFoundException : CustomHttpException
    {
        public UserNotFoundException() : base("There is an issue with your Deloitte OPM Profile that must be resolved. Please contact Deloitte OPM Support.", HttpStatusCode.BadRequest)
        {
        }
    }
}
