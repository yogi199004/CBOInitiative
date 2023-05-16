namespace AAPS.L10nPortal.Dal.Exceptions
{
    [Serializable]
    public class LocaleNotFoundException : CustomSqlException
    {
        public LocaleNotFoundException() : base("Application Locale not found.")
        {
        }
    }
}