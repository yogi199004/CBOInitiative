using CAPPortal.Contracts.Providers;
using CAPPortal.Entities;
using CAPPortal.Secrets;
using CAPPortal.Common;
using Microsoft.Extensions.Configuration;

namespace CAPPortal.Dal
{
    public class OpmDataProvider : AzureApiDataProviderBase, IOpmDataProvider
    {
        private const string OpmApiConfigurationBaseUri = "Removed";
        private const string OpmApiConfigurationResourceId = "Removed";
        private const string OpmApiConfigurationApplicationName = "Removed";
        IConfiguration _config;

        private string _appCode;
        private AzureTokenProvider AzureTokenProvider { get; }

        public OpmDataProvider(IConfiguration config)
        {
            _config = config;
            //BaseUri = new Uri("removed");
            _appCode = "CAPPortal";
            AzureTokenProvider = new AzureTokenProvider(_config, FunctionsConstants.AzureServiceConnectionString, OpmApiConfigurationResourceId);
        }

        public async Task<IEnumerable<GlobalEmployeeUser>> GetByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken),
                                                                            int top = 20, int skip = 0)
        {
            var queryPath = $"Removed/{_appCode}/{top}/{skip}";
            var accessToken = await AzureTokenProvider.GetTokenAsync();
            var result = CallPostApiMethodAsync<IEnumerable<GlobalEmployeeUser>>(queryPath, new List<string> { email },
                                                                                    accessToken, cancellationToken);
            return await result;
        }

        public async Task<IEnumerable<GlobalEmployeeUser>> GetByUidListAsync(IEnumerable<Guid> uidList, CancellationToken cancellationToken = default(CancellationToken))
        {
            var queryPath = $"Removed/{_appCode}";
            var accessToken = await AzureTokenProvider.GetTokenAsync();
            var ids = uidList?.Select(x => x.ToString()).ToList();

            var result = new List<GlobalEmployeeUser>();
            // TODO: Change to single call when OpmApi returns more than 100 items
            const int pageSize = 100;
            for (int i = 0; i < ids.Count; i += pageSize)
            {
                var idsPage = ids.GetRange(i, Math.Min(pageSize, ids.Count - i));
                var resultChunk = (await CallPostApiMethodAsync<IEnumerable<GlobalEmployeeUser>>(queryPath, idsPage,
                                                                                        accessToken, cancellationToken))?.ToList();
                result.AddRange(resultChunk);
            }

            return result;
        }

        protected override string GetClientName()
        {
            return _appCode;
        }
    }
}
