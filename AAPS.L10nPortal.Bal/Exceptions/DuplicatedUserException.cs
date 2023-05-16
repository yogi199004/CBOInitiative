using System.Net;

namespace AAPS.L10nPortal.Bal.Exceptions
{
    [Serializable]
    public class DuplicatedUserException : CustomHttpException
    {
        public DuplicatedUserException() : base("There is an issue with your Deloitte OPM Profile that must be resolved. Please contact Deloitte OPM Support.", HttpStatusCode.BadRequest)
        {
        }
    }
}
