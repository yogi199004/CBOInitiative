namespace CAPPortal.Dal.Exceptions
{
    [Serializable]
    public class ApplicationLocaleAlreadyAssignedException : CustomSqlException
    {
        public ApplicationLocaleAlreadyAssignedException() : base("Application Locale already assigned.")
        {
        }
    }
}
