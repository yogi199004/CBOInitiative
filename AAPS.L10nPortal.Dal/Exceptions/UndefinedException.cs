namespace CAPPortal.Dal.Exceptions
{
    [Serializable]
    public class UndefinedException : CustomSqlException
    {
        public UndefinedException()
        {
        }

        public UndefinedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}