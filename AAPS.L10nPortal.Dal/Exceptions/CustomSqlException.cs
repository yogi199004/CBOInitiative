namespace AAPS.L10nPortal.Dal.Exceptions
{
    [Serializable]
    public class CustomSqlException : Exception
    {
        public CustomSqlException()
        {
        }

        public CustomSqlException(string message) : base(message)
        {
        }

        public CustomSqlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}