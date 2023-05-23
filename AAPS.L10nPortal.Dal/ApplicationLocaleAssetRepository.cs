﻿using AAPS.L10nPortal.Contracts.Repositories;
using AAPS.L10nPortal.Contracts.Services;
using AAPS.L10nPortal.Entities;
using DbDataReaderMapper;
using System.Data;
using System.Data.SqlClient;

namespace AAPS.L10nPortal.Dal
{
    public class ApplicationLocaleAssetRepository : L10nBaseRepository, IApplicationLocaleAssetRepository
    {
        public ApplicationLocaleAssetRepository(IConnectionStringProvider connectionStringProvider) : base(connectionStringProvider)
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
        public IEnumerable<UserApplicationLocale> GetUserApplicationLocaleList()
        {
            return new List<UserApplicationLocale>();
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
        public int ApplicationOnboarding(PermissionData permissionData, GlobalEmployeeUser globalEmployeeUser, string applicationName)
        {
            return 0;
        }
        public int AddAppManagerAsync(PermissionData permissionData, int applicationLocaleId, Guid assignToUserId)
        {
            return 0;
        }

        public async Task<IEnumerable<ApplicationLocaleAsset>> GetList(int applicationLocaleId)
        {
            List<ApplicationLocaleAsset> ApplicationLocaleAssetList = new List<ApplicationLocaleAsset>();
            using (var connection = await CreateSqlConnection())
            {

                using (var command = new SqlCommand("spApplicationLocaleAssetGetList", connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@userId", "00000000-0000-0000-0000-00007731eedc");
                    command.Parameters.AddWithValue("@applicationLocaleId", applicationLocaleId);
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var applicationLocaleModelObj = reader.MapToObject<ApplicationLocaleAsset>();
                        ApplicationLocaleAssetList.Add(applicationLocaleModelObj);

                    }
                }
            }

            return ApplicationLocaleAssetList;
        }

        public IEnumerable<ApplicationLocaleAsset> GetAssetKeyWithAsset(PermissionData permissionData, int applicationLocaleId)
        {
            return new List<ApplicationLocaleAsset>();
        }
        public IEnumerable<ApplicationLocaleAsset> Update(PermissionData permissionData, int applicationLocaleId, int keyId, string value)
        {
            return new List<ApplicationLocaleAsset>();
        }
        public IEnumerable<ApplicationLocaleAsset> Get(PermissionData permissionData, int applicationLocaleId, int keyId)
        {
            return new List<ApplicationLocaleAsset>();
        }


    }
}