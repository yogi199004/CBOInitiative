using CAPPortal.Entities;
using System.Security.Principal;

namespace CAPPortal.Contracts.Services
{
    public interface IPermissionDataService
    {
        PermissionData Get(IPrincipal user);
    }
}
