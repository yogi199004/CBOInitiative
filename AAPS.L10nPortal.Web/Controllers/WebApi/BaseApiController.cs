using CAPPortal.Bal.Exceptions;
using CAPPortal.Contracts.Services;
using CAPPortal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CAPPortal.Web.Controllers.WebApi
{
    [Authorize]
    //[ApiController]
    public class BaseApiController : Controller
    {
        private ILogger<BaseApiController> Logger;
        private IPermissionDataService PermissionDataService;
        public BaseApiController(IPermissionDataService permissionDataService)
        {
            PermissionDataService = permissionDataService;

        }



        protected PermissionData CreatePermissionData()
        {
            try
            {
                return PermissionDataService.Get(User);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                //Logger.LogError(ex, GetUserInfo(User.Identity.Name));
                if (ex is UserNotFoundException || ex is DuplicatedUserException)
                {
                    throw;
                }
                return null;

                //throw;


            }
        }

        public string GetUserInfo(string userEmail)
        {
            var userInfo = $"User Email: {userEmail}";
            return userInfo;
        }
    }
}
