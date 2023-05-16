namespace AAPS.L10nPortal.Web.Models
{
    public class ErrorMessageResult
    {

        public ErrorMessageResult(int statuscode, string message)
        {
            StatusCode = statuscode;
            Message = message;

        }

        public int StatusCode { get; set; }


        public string Message { get; set; }




    }
}