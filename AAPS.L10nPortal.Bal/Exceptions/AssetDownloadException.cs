using System.Net;

namespace AAPS.CAPPortal.Bal.Exceptions
{
    [Serializable]
    public class AssetDownloadException : CustomHttpException
    {
        public AssetDownloadException(Exception inner)
            : base(inner, "Error occured while downloading asset", HttpStatusCode.BadRequest)
        {
        }
    }
}
