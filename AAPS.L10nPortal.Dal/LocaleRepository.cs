using CAPPortal.Contracts.Repositories;
using CAPPortal.Contracts.Services;
using CAPPortal.Entities;


namespace CAPPortal.Dal
{
    public class LocaleRepository : CAPBaseRepository, ILocaleRepository
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
