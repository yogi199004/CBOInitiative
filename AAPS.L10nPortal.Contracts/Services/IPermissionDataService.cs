using AAPS.L10nPortal.Entities;
using System.Security.Principal;

namespace AAPS.L10nPortal.Contracts.Services
{
    public interface IPermissionDataService
    {
        PermissionData Get(IPrincipal user);
    }
}
