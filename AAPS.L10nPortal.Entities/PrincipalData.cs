namespace AAPS.L10nPortal.Entities
{
    public class PrincipalData
    {
        public PrincipalData(string userEmail)
        {
            UserEmail = userEmail;
        }

        public string UserEmail { get; }
    }
}
