using CAPPortal.Contracts.Services;
using System.Data;
using System.Data.SqlClient;

namespace CAPPortal.Dal
{
    public abstract class BaseRepository
    {
        private IConnectionStringProvider ConnectionStringProvider;

        private readonly string connectionStringName;
        private readonly string passwordSecretName;

        protected BaseRepository(IConnectionStringProvider connectionStringProvider, string connectionString, string passwordSecretName)
        {
            this.connectionStringName = connectionString;
            this.passwordSecretName = passwordSecretName;

            ConnectionStringProvider = connectionStringProvider;
        }

        public async Task<SqlConnection> CreateSqlConnection()
        {
            //var connectionString = "Data Source=localhost;Initial Catalog=L10nPortal;Integrated Security=True";
            var connectionString = await ConnectionStringProvider.GetConnectionString(connectionStringName, passwordSecretName);
            

            return new SqlConnection(connectionString);
        }

        protected async Task<T> ExecuteReaderAsync<T>(
            string storedProcedureName,
            Func<SqlDataReader, Task<T>> mapper,
            Action<SqlParameterCollection> applyStorProcParameters = null,
            CancellationToken token = default(CancellationToken),
            int timeOutSeconds = 90)
        {
            return await ProcessCommandAsync(storedProcedureName, applyStorProcParameters,
                async command =>
                {
                    using (var reader =
                        await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, token).ConfigureAwait(false))
                    {
                        return await mapper(reader);
                    }
                },
                token,
                timeOutSeconds);
        }

        protected async Task<int> ExecuteNonQueryAsync(
            string storedProcedureName,
            Action<SqlParameterCollection> applyStorProcParameters = null,
            CancellationToken token = default(CancellationToken))
        {
            return await ProcessCommandAsync(storedProcedureName, applyStorProcParameters,
                async command => await command.ExecuteNonQueryAsync(token), token).ConfigureAwait(false);
        }

        protected async Task<object> ExecuteScalarAsync(
            string storedProcedureName,
            Action<SqlParameterCollection> applyStorProcParameters = null,
            CancellationToken token = default(CancellationToken))
        {
            return await ProcessCommandAsync(storedProcedureName, applyStorProcParameters,
                async command => await command.ExecuteScalarAsync(token), token).ConfigureAwait(false);
        }

        protected async Task<T> ExecuteScalarAsync<T>(string storedProcedureName,
            Action<SqlParameterCollection> applyStorProcParameters = null,
            CancellationToken token = default(CancellationToken))
        {
            var result = await ExecuteScalarAsync(storedProcedureName, applyStorProcParameters, token);

            if (result == null || result == DBNull.Value)
            {
                return default(T);
            }

            return (T)result;
        }

        private async Task<T> ProcessCommandAsync<T>(
            string storedProcedureName,
            Action<SqlParameterCollection> applyStorProcParameters,
            Func<SqlCommand, Task<T>> processCommand,
            CancellationToken token = default(CancellationToken),
            int timeOutSeconds = 90)
        {

            using (var connection = await CreateSqlConnection())
            {
                await connection.OpenAsync(token).ConfigureAwait(false);

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = timeOutSeconds;

                    if (applyStorProcParameters != null)
                    {
                        applyStorProcParameters(command.Parameters);
                        foreach (var parameter in command.Parameters.OfType<SqlParameter>().Where(p => p.DbType == DbType.DateTime))
                        {
                            parameter.DbType = DbType.DateTime2;
                        }
                    }

                    try
                    {
                        return await processCommand(command);
                    }
                    catch (AggregateException ex)
                    {
                        ex.Handle(
                            innerEx =>
                            {
                                throw ex;
                                if (innerEx is SqlException sqlEx) throw DataAccessExceptionHelper.Create(sqlEx);
                                return false;
                            });

                        // this code never executes
                        return default(T);
                    }
                    catch (SqlException ex)
                    {
                        // throw ex;
                        throw DataAccessExceptionHelper.Create(ex);
                    }
                }
            }
        }
    }
}
