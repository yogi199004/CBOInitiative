namespace CAPPortal.Contracts.Services
{
    public interface IConnectionStringProvider
    {
        Task<string> GetConnectionString(string connectionStringName, string passwordSecretName);
    }
}
