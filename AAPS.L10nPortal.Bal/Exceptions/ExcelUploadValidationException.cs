using System.Net;

namespace CAPPortal.Bal.Exceptions
{
    [Serializable]
    public class ExcelUploadValidationException : CustomHttpException
    {
        public ExcelUploadValidationException(string message)
            : base(message, HttpStatusCode.BadRequest)
        {
        }
    }
}
