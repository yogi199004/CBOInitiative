using CAPPortal.Entities;
using System.Security.Principal;

namespace CAPPortal.Contracts.Services
{
    public interface IPrincipalDataService
    {
        PrincipalData Get(IPrincipal user);
    }
}
