using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Contracts.Repositories
{
    public interface IApplicationLocaleResourceKeyValueRepository
    {
        Task<IEnumerable<ApplicationLocaleResourceKeyValue>> GetListAsync(int applicationId, int localeId);
    }
}
