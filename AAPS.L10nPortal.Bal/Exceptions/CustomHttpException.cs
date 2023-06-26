using System.Net;

namespace CAPPortal.Bal.Exceptions
{
    [Serializable]
    public class CustomHttpException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public bool Legacy { get; }

        public object ExceptionData { get; }

        public CustomHttpException(Exception inner, string message, HttpStatusCode statusCode, object exceptionData, bool legacy) : base(message, inner)
        {
            StatusCode = statusCode;
            Legacy = legacy;
            ExceptionData = exceptionData;
        }

        public CustomHttpException(Exception inner, string message, HttpStatusCode statusCode, bool legacy) : this(inner, message, statusCode, null, legacy)
        {
        }

        public CustomHttpException(string message, HttpStatusCode statusCode, object exceptionData) : this(null, message, statusCode, exceptionData, false)
        {
        }

        public CustomHttpException(Exception inner, string message, HttpStatusCode statusCode) : this(inner, message, statusCode, false)
        {
        }

        public CustomHttpException(string message, HttpStatusCode statusCode, bool legacy) : this(null, message, statusCode, legacy)
        {
        }

        public CustomHttpException(string message, HttpStatusCode statusCode) : this(message, statusCode, false)
        {
        }
    }
}
