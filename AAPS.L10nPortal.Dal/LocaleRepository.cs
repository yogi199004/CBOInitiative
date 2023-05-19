using AAPS.L10nPortal.Contracts.Repositories;
using AAPS.L10nPortal.Contracts.Services;
using AAPS.L10nPortal.Entities;


namespace AAPS.L10nPortal.Dal
{
    public class LocaleRepository : L10nBaseRepository, ILocaleRepository
    {
        public LocaleRepository(IConnectionStringProvider connectionStringProvider) : base(connectionStringProvider)
        {


        }

        public IEnumerable<Locale> GetLocalesList()
        {
            return new List<Locale>();
        }
    }
}
