using CAPPortal.Contracts.Managers;
using CAPPortal.Contracts.Services;
using CAPPortal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CAPPortal.Web.Controllers.WebApi
{
    [Route("api/Locale")]
    public class LocaleController : BaseApiController
    {
        private ILocaleManager LocaleManager { get; }

        private ILogger<LocaleController> Logger { get; }

        public LocaleController(ILogger<LocaleController> _log, IPermissionDataService permissionDataService, ILocaleManager localeManager) : base(permissionDataService)
        {
            this.LocaleManager = localeManager;
            Logger = _log;

        }
        //[AllowAnonymous]
        [HttpGet]
        public  IEnumerable<Locale> GetLocalesList()
        {

            try
            {
                return LocaleManager.GetLocalesList();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;

            }
        }

    }
}
