namespace AAPS.L10nPortal.Entities
{
    public class PortalUser : GlobalEmployeeUser
    {
        public PortalUser()
        {

        }

        public PortalUser(GlobalEmployeeUser globalEmployee)
        {
            GlobalPersonUid = globalEmployee.GlobalPersonUid;
            MemberFirmCode = globalEmployee.MemberFirmCode;
            CountryCode = globalEmployee.CountryCode;
            PreferredFullName = globalEmployee.PreferredFullName;
            Email = globalEmployee.Email;
        }

        public IEnumerable<Application> ApplicationManager { get; set; }
    }
}
