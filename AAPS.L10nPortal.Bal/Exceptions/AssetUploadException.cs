using System.Net;

namespace CAPPortal.Bal.Exceptions
{
    [Serializable]
    public class AssetUploadException : CustomHttpException
    {
        public AssetUploadException(Exception inner)
            : base(inner, "Error occured while uploading asset", HttpStatusCode.BadRequest)
        {
        }
    }
}
