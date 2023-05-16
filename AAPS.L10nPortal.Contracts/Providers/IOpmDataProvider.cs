using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Contracts.Providers
{
    public interface IOpmDataProvider
    {
        Task<IEnumerable<GlobalEmployeeUser>> GetByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken), int top = 20, int skip = 0);

        Task<IEnumerable<GlobalEmployeeUser>> GetByUidListAsync(IEnumerable<Guid> uidList, CancellationToken cancellationToken = default(CancellationToken));
    }
}
