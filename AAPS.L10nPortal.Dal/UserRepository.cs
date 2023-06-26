using CAPPortal.Contracts.Repositories;
using CAPPortal.Contracts.Services;
using CAPPortal.Entities;
using System.Data.SqlClient;
using System.Data;
using DbDataReaderMapper;

namespace CAPPortal.Dal
{
    public class UserRepository : CAPBaseRepository, IUserRepository
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


        public async Task<IEnumerable<GlobalEmployeeUser>> ResolveUser(string email)
        {
            List<GlobalEmployeeUser> globalEmployeeUserList = new List<GlobalEmployeeUser>();
            using (var connection = await CreateSqlConnection())
            {

                using (var command = new SqlCommand("spResolveUser", connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@email", email);
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var globalEmployeeUser = reader.MapToObject<GlobalEmployeeUser>();
                        globalEmployeeUserList.Add(globalEmployeeUser);

                    }
                }
            }

            return globalEmployeeUserList;
        }

    }
}
