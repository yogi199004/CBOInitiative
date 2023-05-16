using L10N.API.SyncFunction.Model;
using System.Data;
using System.Data.SqlClient;

namespace L10N.API.SyncFunction.DAL
{
    public class FunctionDataProvider
    {
        public List<Application> GetL10NApps(string connectionString)
        {
            List<Application> lstapplication = new List<Application>();
            SqlConnection cn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("spGetApplications", cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Application app = new Application();
                app.Name = Convert.ToString(rdr[0]);
                app.Locale = Convert.ToString(rdr[1]);
                lstapplication.Add(app);
            }
            rdr.Close();
            cn.Close();

            return lstapplication;
        }

        public async Task<List<MetaDataModel>> GetAppSpecificMetaData(string appName, string connectionString)
        {
            AppMetaData values = new AppMetaData();

            int rowCount = 0;
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("spGetAppSpecificMetaData", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@appName", appName);
                connection.Open();
                using (Task<SqlDataReader> dr = cmd.ExecuteReaderAsync())
                {
                    table.Load(await dr);
                }
                connection.Close();
            }

            if (table.Rows.Count >= 1)
            {

                values.AppName = Convert.ToString(table.Rows[0][0]) + "_" + "MetaData";
                values.id = System.Guid.NewGuid();
                values.MetaData = new List<MetaDataModel>();
                foreach (DataRow dr in table.Rows)
                {
                    values.MetaData.Add(new MetaDataModel
                    {
                        Language = Convert.ToString(dr[1]),
                        LocaleCode = Convert.ToString(dr[2]),
                        NativeName = Convert.ToString(dr[3]),
                        EnglishName = Convert.ToString(dr[4]),
                        NativeLanguageName = Convert.ToString(dr[5]),
                        EnglishLanguageName = Convert.ToString((dr[6])),
                        NativeCountryName = Convert.ToString(dr[7]),
                        EnglishCountryName = Convert.ToString(dr[8]),
                        LastModifiedDate = (dr[9] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr[9]))

                    }); ;

                }
            }


            return values.MetaData;

        }

        public async Task<List<MetaDataModel>> GetAppsMetaData(string connectionString)
        {
            AppMetaData values = new AppMetaData();

            int rowCount = 0;
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("spGetAppsMetaData", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //cmd.Parameters.AddWithValue("@appName", appName);
                connection.Open();
                using (Task<SqlDataReader> dr = cmd.ExecuteReaderAsync())
                {
                    table.Load(await dr);
                }
                connection.Close();
            }

            if (table.Rows.Count >= 1)
            {

                // values.AppName = Convert.ToString(table.Rows[0][0]) + "_" + "MetaData";
                values.id = System.Guid.NewGuid();
                values.MetaData = new List<MetaDataModel>();
                foreach (DataRow dr in table.Rows)
                {
                    values.MetaData.Add(new MetaDataModel
                    {
                        AppName = Convert.ToString(dr[0]),
                        Language = Convert.ToString(dr[1]),
                        LocaleCode = Convert.ToString(dr[2]),
                        NativeName = Convert.ToString(dr[3]),
                        EnglishName = Convert.ToString(dr[4]),
                        NativeLanguageName = Convert.ToString(dr[5]),
                        EnglishLanguageName = Convert.ToString((dr[6])),
                        NativeCountryName = Convert.ToString(dr[7]),
                        EnglishCountryName = Convert.ToString(dr[8]),
                        LastModifiedDate = (dr[9] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr[9]))

                    }); ;

                }
            }

            return values.MetaData;

        }


        public async Task<List<App10NKeysandValuescs>> GetAppsKeyValues(string connectionString)
        {

            App10NKeysandValuescs values = new App10NKeysandValuescs();
            List<App10NKeysandValuescs> appKeyValues = new List<App10NKeysandValuescs>();
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAppsLocaleSpecificKeyValues", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                using (Task<SqlDataReader> dr = cmd.ExecuteReaderAsync())
                {
                    table.Load(await dr);
                }
                connection.Close();
            }

            if (table.Rows.Count >= 1)
            {

                appKeyValues = (from DataRow dr in table.Rows
                                select new App10NKeysandValuescs()
                                {
                                    AppName = dr["applicationname"].ToString(),
                                    LocaleCode = dr["LocaleCode"].ToString(),
                                    ResourcKey = dr["ResourceKey"].ToString(),
                                    LocaleValue = dr["LocaleValue"].ToString(),
                                    CreatedDate = dr["CreatedDate"] == DBNull.Value ? null : Convert.ToDateTime(dr["CreatedDate"]),
                                    UpdatedDate = dr["UpdatedDate"] == DBNull.Value ? null : Convert.ToDateTime(dr["UpdatedDate"]),
                                    CreatedBy = dr["CreatedBy"] == DBNull.Value ? null : new Guid(dr["CreatedBy"].ToString()),
                                    UpdatedBy = dr["UpdatedBy"] == DBNull.Value ? null : new Guid(dr["UpdatedBy"].ToString())
                                }).ToList();


                return appKeyValues;

            }
            else
            {
                return null;
            }


        }

        public async Task<List<App10NKeysandValuescs>> GetAppsKeyValuesForDeletedApps(string connectionString, string appName)
        {

            App10NKeysandValuescs values = new App10NKeysandValuescs();
            List<App10NKeysandValuescs> appKeyValues = new List<App10NKeysandValuescs>();
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetKeyValuesForAppWithDeletedKey", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@appName", appName);
                connection.Open();
                using (Task<SqlDataReader> dr = cmd.ExecuteReaderAsync())
                {
                    table.Load(await dr);
                }
                connection.Close();
            }

            if (table.Rows.Count >= 1)
            {

                appKeyValues = (from DataRow dr in table.Rows
                                select new App10NKeysandValuescs()
                                {
                                    AppName = dr["applicationname"].ToString(),
                                    LocaleCode = dr["LocaleCode"].ToString(),
                                    ResourcKey = dr["ResourceKey"].ToString(),
                                    LocaleValue = dr["LocaleValue"].ToString(),
                                    CreatedDate = dr["CreatedDate"] == DBNull.Value ? null : Convert.ToDateTime(dr["CreatedDate"]),
                                    UpdatedDate = dr["UpdatedDate"] == DBNull.Value ? null : Convert.ToDateTime(dr["UpdatedDate"]),
                                    CreatedBy = dr["CreatedBy"] == DBNull.Value ? null : new Guid(dr["CreatedBy"].ToString()),
                                    UpdatedBy = dr["UpdatedBy"] == DBNull.Value ? null : new Guid(dr["UpdatedBy"].ToString())
                                }).ToList();


                return appKeyValues;

            }
            else
            {
                return null;
            }


        }

        public void UpdateSyncConfiguration(string connectionString)
        {
            SqlConnection cn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("UpdateSyncConfiguration", cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cn.Open();

            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public List<Application> GetDeletedLocales(string connectionString)
        {
            List<Application> lstDeletedAppLocales = new List<Application>();
            SqlConnection cn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("spGetDeletedLocales", cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Application app = new Application();
                app.Name = Convert.ToString(rdr[0]);
                app.Locale = Convert.ToString(rdr[1]);
                lstDeletedAppLocales.Add(app);
            }
            rdr.Close();
            cn.Close();

            return lstDeletedAppLocales;
        }

        public List<Application> GetAppsWithDeletedKeys(string connectionString)
        {
            List<Application> lstAppsWithDeletedKeys = new List<Application>();
            SqlConnection cn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("spGetAppsWithDeletedKey", cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Application app = new Application();
                app.Name = Convert.ToString(rdr[0]);
                lstAppsWithDeletedKeys.Add(app);
            }
            rdr.Close();
            cn.Close();

            return lstAppsWithDeletedKeys;
        }

    }
}
