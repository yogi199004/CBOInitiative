namespace AAPS.L10nPortal.Bal.Exceptions
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
