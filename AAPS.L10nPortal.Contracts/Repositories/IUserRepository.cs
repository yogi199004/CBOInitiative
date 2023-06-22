using AAPS.L10nPortal.Entities;
using System.Collections.Generic;

namespace AAPS.L10nPortal.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(PermissionData permissionData, Guid newUserId, string newUserEmail);

        IEnumerable<Application> GetUserApplicationsAsync(PermissionData permissionData, bool? canManage);

        Task<bool> GetAdminUserAsync(PermissionData permission);

        Task<IEnumerable<GlobalEmployeeUser>> ResolveUser(string email);
    }
}
