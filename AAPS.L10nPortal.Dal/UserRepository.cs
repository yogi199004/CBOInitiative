using AAPS.L10nPortal.Contracts.Repositories;
using AAPS.L10nPortal.Contracts.Services;
using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Dal
{
    public class UserRepository : L10nBaseRepository, IUserRepository
    {

        public UserRepository(IConnectionStringProvider connectionStringProvider) : base(connectionStringProvider)
        {

        }



        public IEnumerable<Application> GetUserApplicationsAsync(PermissionData permissionData, bool? canManage)
        {
            List<Application> a = new List<Application>();
            return a;
            //to do implementation
        }

        public Task<bool> GetAdminUserAsync(PermissionData permission)
        {
            return Task.FromResult(true);
        }

        public async Task CreateUserAsync(PermissionData permissionData, Guid newUserId, string newUserEmail)
        {
            await ExecuteNonQueryAsync("spUserCreate",
                collection =>
                {
                    collection.AddWithValue("@userId", permissionData.UserId);
                    collection.AddWithValue("@newUserId", newUserId);
                    collection.AddWithValue("@newUserEmail", newUserEmail);
                });
        }




    }
}
