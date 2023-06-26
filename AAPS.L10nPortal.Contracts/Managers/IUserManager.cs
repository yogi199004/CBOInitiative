using CAPPortal.Entities;

namespace CAPPortal.Contracts.Managers
{
    public interface IUserManager
    {
        Task CreateUserAsync(PermissionData permissionData, Guid newUserId, string newUserEmail);

        Task<PortalUser> GetCurrent(PermissionData permissionData);

        Task<Dictionary<Guid, GlobalEmployeeUser>> Resolve(IEnumerable<Guid> uids);

        Task<GlobalEmployeeUser> Resolve(PrincipalData user);

        Task<GlobalEmployeeUser> Resolve(string email);

        Task<bool> GetAdminUser(PermissionData permissionData);
    }
}
