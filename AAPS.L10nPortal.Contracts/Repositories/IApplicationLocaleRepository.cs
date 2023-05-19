using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Contracts.Repositories
{
    public interface IApplicationLocaleRepository
    {
        UserApplicationLocale CreateUserApplicationLocaleAsync(PermissionData permissionData, int applicationId, int localeId, Guid assignTo);
        UserApplicationLocale ReassignApplicationLocaleAsync(PermissionData permissionData, int applicationLocaleId, Guid assignFromUserId, Guid assignToUserId);
        IEnumerable<ApplicationLocaleModel> GetApplicationLocaleListAsync();
        IEnumerable<UserApplicationLocale> GetUserApplicationLocaleList();
        IEnumerable<ApplicationLocaleValue> GetApplicationLocaleValueList(PermissionData permissionData, int applicationLocaleId);
        IEnumerable<UserApplicationLocale> GetUserApplicationLocaleById(PermissionData permissionData, int applicationLocaleId);
        int ApplicationLocaleValueMerge(PermissionData permissionData, int applicationLocaleId, IEnumerable<ApplicationLocaleValue> values);
        int ApplicationOriginalValueMerge(PermissionData permissionData, int applicationLocaleId, IEnumerable<ResourceKeyValue> values);
        int ApplicationLocaleDelete(PermissionData permissionData, int applicationLocaleId);
        int ApplicationOnboarding(PermissionData permissionData, GlobalEmployeeUser globalEmployeeUser, string applicationName);
        int AddAppManagerAsync(PermissionData permissionData, int applicationLocaleId, Guid assignToUserId);

    }
}
