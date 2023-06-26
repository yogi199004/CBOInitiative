using CAPPortal.Contracts.Managers;
using CAPPortal.Contracts.Repositories;
using CAPPortal.Entities;

namespace AAPS.CAPPortal.Bal
{
    public class LocaleManager : ILocaleManager
    {
        private ILocaleRepository LocaleRepository { get; }

        public LocaleManager(ILocaleRepository localeRepository)
        {
            this.LocaleRepository = localeRepository;
        }

        public IEnumerable<Locale> GetLocalesList()
        {
            return LocaleRepository.GetLocalesList();
        }
    }
}
