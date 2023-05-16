namespace AAPS.L10nPortal.Contracts.Providers
{
    public interface IAzureKeyVaultDataProvider
    {
        Task<string> GetSecretAsync(string keyVaultUri, string secretName);
    }
}
