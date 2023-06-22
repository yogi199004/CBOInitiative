using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Contracts.Managers
{
    public interface IApplicationLocaleManager
    {
        Task<UserApplicationLocale> CreateUserApplicationLocaleAsync(PermissionData permissionData, CreateUserApplicationLocaleModel model);
        Task<UserApplicationLocale> ReassignApplicationLocaleAsync(PermissionData permissionData, int applicationLocaleId, Guid assignFromUserId, string reassignToUserEmail);
        Task<IEnumerable<UserApplicationLocale>> GetUserApplicationLocaleListAsync(PermissionData permissionData);
        Task<UserApplicationLocale> GetApplicationLocaleByIdAsync(PermissionData permissionData, int applicationLocaleId);
        Task<IEnumerable<ApplicationLocaleValue>> GetApplicationLocaleValueListAsync(PermissionData permissionData, int applicationLocaleId);
        Task<int> ApplicationLocaleValueMergeAsync(PermissionData permissionData, int applicationLocaleId, IEnumerable<ApplicationLocaleValue> values);
        Task<int> ApplicationOriginalValueMergeAsync(PermissionData permissionData, int applicationLocaleId, IEnumerable<ResourceKeyValue> values);
        Task<int> DeleteApplicationLocaleAsync(PermissionData permissionData, int applicationLocaleId);
        Task<int> ApplicationOnboarding(CreateUserApplicationModel model);
        Task<int> AddAppManagerAsync(PermissionData permissionData, int applicationLocaleId, string reassignToUserEmail);


    }
}
