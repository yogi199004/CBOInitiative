using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Contracts.Managers
{
    public interface IApplicationLocaleResourceKeyValueManager
    {
        Task<IEnumerable<ApplicationLocaleResourceKeyValue>> GetListAsync(int applicationId, int localeId);
    }
}
