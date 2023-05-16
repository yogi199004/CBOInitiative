using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Contracts.Repositories
{
    public interface ILocaleRepository
    {
        IEnumerable<Locale> GetLocalesList();
    }
}
