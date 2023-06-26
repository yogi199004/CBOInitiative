using CAPPortal.Contracts.Services;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace CAPPortal.Common.Services
{
    public class AppConfigConnectionStringProvider : IConnectionStringProvider
    {
        private static IConfiguration config;
        public AppConfigConnectionStringProvider(IConfiguration _config)
        {
            config = _config;

        }

        public Task<string> GetConnectionString(string connectionStringName, string passwordSecretName)
        {
            return Task.FromResult(config.GetConnectionString(connectionStringName));
        }

    }
}
