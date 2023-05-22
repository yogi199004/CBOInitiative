using AAPS.L10nPortal.Contracts.Repositories;
using AAPS.L10nPortal.Contracts.Services;
using AAPS.L10nPortal.Entities;
using System.Data;
using System.Data.SqlClient;

namespace AAPS.L10nPortal.Dal
{
    public class ApplicationLocaleRepository : L10nBaseRepository, IApplicationLocaleRepository
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
        public async void GetUserApplicationLocaleList()
        {
            using (var connection = await CreateSqlConnection())
            {

                using (var command = new SqlCommand("spUserApplicationLocaleGetList", connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@userId", "00000000-0000-0000-0000-00007731eedc");
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        


                    }
                }
            }

            
        }
        public IEnumerable<ApplicationLocaleValue> GetApplicationLocaleValueList(PermissionData permissionData, int applicationLocaleId)
        {
            return new List<ApplicationLocaleValue>();
        }
        public IEnumerable<UserApplicationLocale> GetUserApplicationLocaleById(PermissionData permissionData, int applicationLocaleId)
        {
            return new List<UserApplicationLocale>();
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

        public int ApplicationOnboarding(PermissionData permissionData, GlobalEmployeeUser globalEmployeeUser, string appName)
        {
            return 0;

        }

        public int AddAppManagerAsync(PermissionData permissionData, int applicationLocaleId, Guid assignToUserId)
        {
            return 0;
        }



    }
}
