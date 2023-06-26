using CAPPortal.Entities;

namespace CAPPortal.Contracts.Repositories
{
    public interface IApplicationLocaleRepository
    {
        UserApplicationLocale CreateUserApplicationLocaleAsync(PermissionData permissionData, int applicationId, int localeId, Guid assignTo);
        UserApplicationLocale ReassignApplicationLocaleAsync(PermissionData permissionData, int applicationLocaleId, Guid assignFromUserId, Guid assignToUserId);
        IEnumerable<ApplicationLocaleModel> GetApplicationLocaleListAsync();
        Task<IEnumerable<UserApplicationLocale>> GetUserApplicationLocaleList(PermissionData permissionData);

        //IEnumerable<UserApplicationLocale> GetUserApplicationLocaleList();
        IEnumerable<ApplicationLocaleValue> GetApplicationLocaleValueList(PermissionData permissionData, int applicationLocaleId);
        Task<IEnumerable<UserApplicationLocale>> GetUserApplicationLocaleById( int applicationLocaleId);
        int ApplicationLocaleValueMerge(PermissionData permissionData, int applicationLocaleId, IEnumerable<ApplicationLocaleValue> values);
        int ApplicationOriginalValueMerge(PermissionData permissionData, int applicationLocaleId, IEnumerable<ResourceKeyValue> values);
        int ApplicationLocaleDelete(PermissionData permissionData, int applicationLocaleId);
        Task<int> ApplicationOnboarding( string UserId, string applicationName);
        int AddAppManagerAsync(PermissionData permissionData, int applicationLocaleId, Guid assignToUserId);

    }
}
