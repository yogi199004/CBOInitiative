namespace CAPPortal.Contracts.Providers
{
    public interface IAzureTokenProvider
    {
        Task<string> GetTokenAsync();
    }
}
