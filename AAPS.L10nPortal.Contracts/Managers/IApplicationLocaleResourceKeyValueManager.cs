using CAPPortal.Entities;

namespace CAPPortal.Contracts.Managers
{
    public interface IApplicationLocaleResourceKeyValueManager
    {
        Task<IEnumerable<ApplicationLocaleResourceKeyValue>> GetListAsync(int applicationId, int localeId);
    }
}
