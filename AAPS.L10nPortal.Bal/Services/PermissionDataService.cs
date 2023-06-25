using AAPS.L10nPortal.Contracts.Managers;
using AAPS.L10nPortal.Contracts.Services;
using AAPS.L10nPortal.Entities;
using System.Runtime.ExceptionServices;
using System.Security.Principal;

namespace AAPS.L10nPortal.Bal.Services
{
    public class PermissionDataService : IPermissionDataService
    {
        private IPrincipalDataService PrincipalDataService { get; }


        private IUserManager UserManager { get; }


        public PermissionDataService(IPrincipalDataService principalDataService, IUserManager userManager)
        {
            PrincipalDataService = principalDataService;
            UserManager = userManager;

        }
        public  PermissionData Get(IPrincipal user)
        {
            var principalData = PrincipalDataService.Get(user);
            Guid userId;
            try
            {
                userId = Task.Run(async () => await UserManager.Resolve(principalData).ConfigureAwait(true)).Result.GlobalPersonUid;
                //var result  = await UserManager.Resolve(principalData);
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }

            return new PermissionData(principalData, userId);
        }
    }
}
