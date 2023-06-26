namespace CAPPortal.Dal.Exceptions
{
    [Serializable]
    public class LocaleNotFoundException : CustomSqlException
    {
        public LocaleNotFoundException() : base("Application Locale not found.")
        {
        }
    }
}