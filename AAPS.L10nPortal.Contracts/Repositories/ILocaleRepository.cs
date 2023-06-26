using CAPPortal.Entities;

namespace CAPPortal.Contracts.Repositories
{
    public interface ILocaleRepository
    {
        IEnumerable<Locale> GetLocalesList();
    }
}
