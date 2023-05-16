namespace L10N.API.Contracts
{
    public interface IAzureKeyVaultDataProvider
    {
        Task<string> GetSecretAsync(string keyVaultUri, string secretName);
    }
}
