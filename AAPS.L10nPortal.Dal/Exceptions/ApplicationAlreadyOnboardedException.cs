namespace AAPS.L10nPortal.Dal.Exceptions
{
    [Serializable]
    public class ApplicationAlreadyOnboardedException : CustomSqlException
    {
        public ApplicationAlreadyOnboardedException() : base("Application with Same name already onboarded.")
        {
        }
    }
}