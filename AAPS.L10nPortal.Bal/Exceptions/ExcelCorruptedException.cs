namespace CAPPortal.Bal.Exceptions
{
    [Serializable]
    public class ExcelCorruptedException : Exception
    {
        public ExcelCorruptedException(string message)
            : base(message)
        {
        }
    }
}
