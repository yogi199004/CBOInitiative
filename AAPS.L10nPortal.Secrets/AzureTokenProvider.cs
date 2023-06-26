using CAPPortal.Contracts.Providers;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;

namespace CAPPortal.Secrets
{
    public class AzureTokenProvider : IAzureTokenProvider
    {
        private readonly string _connectionStringKey;
        private readonly string _resourceIdKey;
        private IConfiguration _config;

        public AzureTokenProvider(IConfiguration config, string connectionStringKey, string resourceIdKey)
        {
            _config = config;
            //_connectionStringKey = _config.GetValue<string>("AzureServiceConnection");
            //_resourceIdKey = _config.GetValue<string>("OPMAPI:ResourceId");

            _connectionStringKey = "RunAs=App; AppId=93d84877-57bb-44a3-b9a2-6363031b599c;TenantId=36da45f1-dd2c-4d1f-af13-5abe46b99921;AppKey=lyN~ByuXhD7Ry-zSt__7~5yf-Bv8tiJxS-";
            _resourceIdKey = "c5faac2d-49df-4361-b0b9-c3184540fb81";
        }

        public async Task<string> GetTokenAsync()
        {
            var connectinString = _connectionStringKey;
            var azureServiceTokenProvider = new AzureServiceTokenProvider(connectinString);
            var accessToken = await azureServiceTokenProvider
            .GetAccessTokenAsync(_resourceIdKey).ConfigureAwait(false);
            return accessToken;
        }
    }
}
