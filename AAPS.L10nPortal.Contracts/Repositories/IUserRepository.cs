using CAPPortal.Entities;
using System.Collections.Generic;

namespace CAPPortal.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(PermissionData permissionData, Guid newUserId, string newUserEmail);

        IEnumerable<Application> GetUserApplicationsAsync(PermissionData permissionData, bool? canManage);

        Task<bool> GetAdminUserAsync(PermissionData permission);

        Task<IEnumerable<GlobalEmployeeUser>> ResolveUser(string email);
    }
}
