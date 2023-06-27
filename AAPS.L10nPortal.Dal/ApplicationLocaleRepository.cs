using CAPPortal.Contracts.Repositories;
using CAPPortal.Contracts.Services;
using CAPPortal.Entities;
using DbDataReaderMapper;
using System.Data;
using System.Data.SqlClient;

namespace CAPPortal.Dal
{
    public class ApplicationLocaleRepository : CAPBaseRepository, IApplicationLocaleRepository
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
        public async Task<IEnumerable<UserApplicationLocale>> GetUserApplicationLocaleList(PermissionData permissionData)
        {
            List<UserApplicationLocale> userApplicationLocaleList = new List<UserApplicationLocale>();
            using (var connection = await CreateSqlConnection())
            {

                using (var command = new SqlCommand("spUserApplicationLocaleGetList", connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@userEmail", permissionData.UserEmail);
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var applicationLocaleModelObj = reader.MapToObject<UserApplicationLocale>();
                        userApplicationLocaleList.Add(applicationLocaleModelObj);
                        
                    }
                }
            }

            return userApplicationLocaleList;
        }
        public IEnumerable<ApplicationLocaleValue> GetApplicationLocaleValueList(PermissionData permissionData, int applicationLocaleId)
        {
            return new List<ApplicationLocaleValue>();
        }
        public async Task<IEnumerable<UserApplicationLocale>> GetUserApplicationLocaleById( PermissionData permissionData,int applicationLocaleId)
        {
            List<UserApplicationLocale> userApplicationLocaleList = new List<UserApplicationLocale>();
            using (var connection = await CreateSqlConnection())
            {

                using (var command = new SqlCommand("spUserApplicationLocaleGetById", connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@userId", "00000000-0000-0000-0000-00007731eedc");
                    command.Parameters.AddWithValue("@ApplicationLocaleId", applicationLocaleId);
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var applicationLocaleModelObj = reader.MapToObject<UserApplicationLocale>();
                        userApplicationLocaleList.Add(applicationLocaleModelObj);

                    }
                }
            }

            return userApplicationLocaleList;
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

        public async Task<int> ApplicationOnboarding(PermissionData permissionData, CreateUserApplicationModel model)
        {
            
                using (var connection = await CreateSqlConnection())
                {

                    using (var command = new SqlCommand("spOnboardApplication", connection) { CommandType = CommandType.StoredProcedure })
                    {
                        command.Parameters.AddWithValue("@UserEmail", permissionData.UserEmail);
                        command.Parameters.AddWithValue("@assignToUserEmail", model.Email);
                        command.Parameters.AddWithValue("@ApplicationName", model.ApplicationName);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        return result;
                    
                    }
                }
           


        }

        public int AddAppManagerAsync(PermissionData permissionData, int applicationLocaleId, Guid assignToUserId)
        {
            return 0;
        }



    }
}
