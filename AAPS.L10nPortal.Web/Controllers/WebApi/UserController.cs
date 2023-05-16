using AAPS.L10nPortal.Bal.Exceptions;
using AAPS.L10nPortal.Contracts.Managers;
using AAPS.L10nPortal.Contracts.Services;
using AAPS.L10nPortal.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.L10nPortal.Web.Controllers.WebApi
{
    [Route("api/User")]
    public class UserController : BaseApiController
    {
        private IPrincipalDataService PrincipalDataService;



        private IUserManager UserManager { get; }
        public UserController(IPermissionDataService permissionDataService, IPrincipalDataService principalDataService, IUserManager userManager, IConfiguration _configuration) : base(permissionDataService)
        {
            this.PrincipalDataService = principalDataService;
            this.UserManager = userManager;



        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("Current")]
        public async Task<PortalUser> GetCurrent()
        {

            //Logger.LogInformation("Inside User Controller");
            var permissionData = CreatePermissionData();
            try
            {
                return await UserManager.GetCurrent(permissionData);
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, ex.Message);
                if (ex is UserNotFoundException || ex is DuplicatedUserException)
                {
                    throw;
                }
                return null;
            }
        }


        [HttpGet]
        [Route("RenewSession")]
        public IActionResult RenewSession()
        {
            return Ok();
        }


        [HttpGet]
        [Route("AdminUser")]
        public async Task<bool> AdminUser()
        {
            var permissionData = CreatePermissionData();
            try
            {
                var Isadmin = await UserManager.GetAdminUser(permissionData);
                return Isadmin;
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
