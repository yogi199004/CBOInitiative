using AAPS.L10nPortal.Contracts.Repositories;
using AAPS.L10nPortal.Contracts.Services;
using AAPS.L10nPortal.Entities;




namespace AAPS.L10nPortal.Dal
{
    public class ApplicationLocaleRepository : L10nBaseRepository, IApplicationLocaleRepository
    {

        public ApplicationLocaleRepository(IConnectionStringProvider connectionStringProvider) : base(connectionStringProvider)
        {

        }


        public UserApplicationLocale CreateUserApplicationLocaleAsync(PermissionData permissionData, int applicationId, int localeId, Guid assignTo)
        {
            return new UserApplicationLocale();
        }
        public UserApplicationLocale ReassignApplicationLocaleAsync(PermissionData permissionData, int applicationLocaleId, Guid assignFromUserId, Guid assignToUserId)
        {
            return new UserApplicationLocale();
        }
        public IEnumerable<ApplicationLocaleModel> GetApplicationLocaleListAsync()
        {
            return new List<ApplicationLocaleModel>();
        }
        public IEnumerable<UserApplicationLocale> GetUserApplicationLocaleList()
        {
            return new List<UserApplicationLocale>();
        }
        public IEnumerable<ApplicationLocaleValue> GetApplicationLocaleValueList(PermissionData permissionData, int applicationLocaleId)
        {
            return new List<ApplicationLocaleValue>();
        }
        public IEnumerable<UserApplicationLocale> GetUserApplicationLocaleById(PermissionData permissionData, int applicationLocaleId)
        {
            return new List<UserApplicationLocale>();
        }




        public int ApplicationLocaleValueMerge(PermissionData permissionData, int applicationLocaleId, IEnumerable<ApplicationLocaleValue> values)
        {
            return 0;
        }

        public int ApplicationOriginalValueMerge(PermissionData permissionData, int applicationLocaleId, IEnumerable<ResourceKeyValue> values)
        {
            return 0;
        }

        public int ApplicationLocaleDelete(PermissionData permissionData, int applicationLocaleId)
        {
            return 0;
        }

        public int ApplicationOnboarding(PermissionData permissionData, GlobalEmployeeUser globalEmployeeUser, string appName)
        {
            return 0;

        }

        public int AddAppManagerAsync(PermissionData permissionData, int applicationLocaleId, Guid assignToUserId)
        {
            return 0;
        }



    }
}
