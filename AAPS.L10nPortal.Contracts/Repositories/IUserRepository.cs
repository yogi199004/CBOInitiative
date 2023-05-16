using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(PermissionData permissionData, Guid newUserId, string newUserEmail);

        IEnumerable<Application> GetUserApplicationsAsync(PermissionData permissionData, bool? canManage);

        Task<bool> GetAdminUserAsync(PermissionData permission);
    }
}
