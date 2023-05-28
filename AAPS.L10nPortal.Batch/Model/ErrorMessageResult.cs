﻿namespace AAPS.L10nPortal.Batch.Model
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
