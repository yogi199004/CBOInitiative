namespace AAPS.L10nPortal.Contracts.Providers
{
    public interface IAzureTokenProvider
    {
        Task<string> GetTokenAsync();
    }
}
