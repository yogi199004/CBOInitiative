using System.Net;

namespace AAPS.L10nPortal.Bal.Exceptions
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
