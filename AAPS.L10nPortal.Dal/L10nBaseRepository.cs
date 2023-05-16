using AAPS.L10nPortal.Contracts.Services;

namespace AAPS.L10nPortal.Dal
{
    public class L10nBaseRepository : BaseRepository
    {
        public L10nBaseRepository(IConnectionStringProvider connectionStringProvider) : base(connectionStringProvider, "L10nPortal", "L10nServerPassword")
        {

        }
    }
}
