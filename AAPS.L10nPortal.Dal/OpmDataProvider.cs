using AAPS.L10nPortal.Contracts.Providers;
using AAPS.L10nPortal.Entities;
using AAPS.L10nPortal.Secrets;
using AAPS.L10NPortal.Common;
using Microsoft.Extensions.Configuration;

namespace AAPS.L10nPortal.Dal
{
    public class OpmDataProvider : AzureApiDataProviderBase, IOpmDataProvider
    {
        private const string OpmApiConfigurationBaseUri = "OpmApiConfiguration:BaseUri";
        private const string OpmApiConfigurationResourceId = "OpmApiConfiguration:ResourceId";
        private const string OpmApiConfigurationApplicationName = "OpmApiConfiguration:ApplicationName";
        IConfiguration _config;

        private string _appCode;
        private AzureTokenProvider AzureTokenProvider { get; }

        public OpmDataProvider(IConfiguration config)
        {
            _config = config;
            BaseUri = new Uri("https://ddcaopmapiame.aaps.deloitte.com/api/");
            _appCode = "L10nPortal";
            AzureTokenProvider = new AzureTokenProvider(_config, FunctionsConstants.AzureServiceConnectionString, OpmApiConfigurationResourceId);
        }

        public async Task<IEnumerable<GlobalEmployeeUser>> GetByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken),
                                                                            int top = 20, int skip = 0)
        {
            var queryPath = $"GetGlobalEmployeeByEmailIds/{_appCode}/{top}/{skip}";
            var accessToken = await AzureTokenProvider.GetTokenAsync();
            var result = CallPostApiMethodAsync<IEnumerable<GlobalEmployeeUser>>(queryPath, new List<string> { email },
                                                                                    accessToken, cancellationToken);
            return await result;
        }

        public async Task<IEnumerable<GlobalEmployeeUser>> GetByUidListAsync(IEnumerable<Guid> uidList, CancellationToken cancellationToken = default(CancellationToken))
        {
            var queryPath = $"GetGlobalEmployeeByUids/{_appCode}";
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
