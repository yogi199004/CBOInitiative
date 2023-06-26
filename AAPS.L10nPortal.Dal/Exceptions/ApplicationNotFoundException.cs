namespace CAPPortal.Dal.Exceptions
{
    [Serializable]
    public class ApplicationNotFoundException : CustomSqlException
    {
        public ApplicationNotFoundException() : base("Application not found.")
        {
        }
    }
}