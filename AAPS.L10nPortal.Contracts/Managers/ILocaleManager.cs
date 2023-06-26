using CAPPortal.Entities;

namespace CAPPortal.Contracts.Managers
{
    public interface ILocaleManager
    {
        IEnumerable<Locale> GetLocalesList();
    }
}
