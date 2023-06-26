using CAPPortal.Contracts.Services;

namespace CAPPortal.Dal
{
    public class CAPBaseRepository : BaseRepository
    {
        public CAPBaseRepository(IConnectionStringProvider connectionStringProvider) : base(connectionStringProvider, "CAPPortal", "CAPServerPassword")
        {

        }
    }
}
