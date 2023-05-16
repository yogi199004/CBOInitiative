namespace AAPS.L10nPortal.Dal.Exceptions
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