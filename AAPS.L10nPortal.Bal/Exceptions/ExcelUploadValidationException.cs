using System.Net;

namespace AAPS.CAPPortal.Bal.Exceptions
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
