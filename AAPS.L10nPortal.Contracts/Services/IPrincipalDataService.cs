using AAPS.L10nPortal.Entities;
using System.Security.Principal;

namespace AAPS.L10nPortal.Contracts.Services
{
    public interface IPrincipalDataService
    {
        PrincipalData Get(IPrincipal user);
    }
}
