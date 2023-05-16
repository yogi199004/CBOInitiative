using AAPS.L10nPortal.Contracts.Managers;
using AAPS.L10nPortal.Contracts.Repositories;
using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Bal
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
