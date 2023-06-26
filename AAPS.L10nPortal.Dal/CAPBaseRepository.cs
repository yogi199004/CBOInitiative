using AAPS.L10nPortal.Contracts.Services;

namespace AAPS.L10nPortal.Dal
{
    public class CAPBaseRepository : BaseRepository
    {
        public CAPBaseRepository(IConnectionStringProvider connectionStringProvider) : base(connectionStringProvider, "CAPPortal", "CAPServerPassword")
        {

        }
    }
}
