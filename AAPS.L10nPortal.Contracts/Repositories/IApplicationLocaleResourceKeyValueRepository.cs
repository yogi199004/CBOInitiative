using CAPPortal.Entities;

namespace CAPPortal.Contracts.Repositories
{
    public interface IApplicationLocaleResourceKeyValueRepository
    {
        Task<IEnumerable<ApplicationLocaleResourceKeyValue>> GetListAsync(int applicationId, int localeId);
    }
}
