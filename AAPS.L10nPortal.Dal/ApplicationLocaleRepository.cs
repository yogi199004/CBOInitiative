﻿using AAPS.L10nPortal.Contracts.Repositories;
using AAPS.L10nPortal.Contracts.Services;
using AAPS.L10nPortal.Entities;
using DbDataReaderMapper;
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
        public async Task<IEnumerable<UserApplicationLocale>> GetUserApplicationLocaleList()
        {
            List<UserApplicationLocale> userApplicationLocaleList = new List<UserApplicationLocale>();
            using (var connection = await CreateSqlConnection())
            {

                using (var command = new SqlCommand("spUserApplicationLocaleGetList", connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@userId", "00000000-0000-0000-0000-00007731eedc");
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
        public async Task<IEnumerable<UserApplicationLocale>> GetUserApplicationLocaleById( int applicationLocaleId)
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

        public async Task<int> ApplicationOnboarding(string UserId, string appName)
        {
            try
            {
                using (var connection = await CreateSqlConnection())
                {

                    using (var command = new SqlCommand("spOnboardApplication", connection) { CommandType = CommandType.StoredProcedure })
                    {
                        command.Parameters.AddWithValue("@UserId", "00000000-0000-0000-0000-00007731eedc");
                        command.Parameters.AddWithValue("@assignToUserId", "00000000-0000-0000-0000-00007731eedc");
                        command.Parameters.AddWithValue("@ApplicationName", appName);
                        connection.Open();
                        var result = await command.ExecuteNonQueryAsync();
                        return result;
                    }
                }
            }
            catch(Exception ex)
            {
                return -1;
            }


        }

        public int AddAppManagerAsync(PermissionData permissionData, int applicationLocaleId, Guid assignToUserId)
        {
            return 0;
        }



    }
}
