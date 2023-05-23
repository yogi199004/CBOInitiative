using AAPS.L10nPortal.Contracts.Services;
using System.Configuration;

namespace AAPS.L10NPortal.Common.Services
{
    public class AppConfigConnectionStringProvider : IConnectionStringProvider
    {

        public Task<string> GetConnectionString(string connectionStringName, string passwordSecretName)
        {
            //return Task.FromResult(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
            return Task.FromResult("Data Source=localhost;Initial Catalog=L10nPortal;Integrated Security=True");
        }

    }
}
