using System.Net;

namespace AAPS.L10nPortal.Bal.Exceptions
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
