using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Contracts.Managers
{
    public interface ILocaleManager
    {
        IEnumerable<Locale> GetLocalesList();
    }
}
