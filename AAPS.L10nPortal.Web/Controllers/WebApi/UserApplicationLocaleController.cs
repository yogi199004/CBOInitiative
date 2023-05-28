﻿using AAPS.L10nPortal.Contracts.Managers;
using AAPS.L10nPortal.Contracts.Services;
using AAPS.L10nPortal.Entities;
using AAPS.L10NPortal.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace AAPS.L10nPortal.Web.Controllers.WebApi
{
    [Route("api/UserApplicationLocale")]
    public class UserApplicationLocaleController : BaseApiController
    {
        private IApplicationLocaleManager ApplicationLocaleManager { get; }

        private ITranslationExchangeManager TranslationExchangeManager { get; }


        private readonly LogApi Logapi;

        public UserApplicationLocaleController(IPermissionDataService permissionDataService, IApplicationLocaleManager applicationLocaleManager, ITranslationExchangeManager translationExchangeManager, IOptions<AppSettings> appSettings) : base(permissionDataService)
        {
            ApplicationLocaleManager = applicationLocaleManager;
            TranslationExchangeManager = translationExchangeManager;
            Logapi = new LogApi(appSettings?.Value);

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<UserApplicationLocale>> Get()
        {

            try
            {
                Logapi.WriteToLog("Application Started", LogLevelEnum.Information);
                return await this.ApplicationLocaleManager.GetUserApplicationLocaleListAsync();
            }
            catch (Exception ex)
            {
                Logapi.WriteToLog(ex.GetBaseException().Message, LogLevelEnum.Error);
                return null;
            }
        }



        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public Task<UserApplicationLocale> Create([FromBody] CreateUserApplicationLocaleModel model)
        {
            var permissionData = CreatePermissionData();

            try
            {
                return ApplicationLocaleManager.CreateUserApplicationLocaleAsync(permissionData, model);
            }
            catch (Exception ex)
            {
                Logapi.WriteToLog(GetUserInfo(permissionData?.UserEmail), LogLevelEnum.Information);
                Logapi.WriteToLog(ex.GetBaseException().Message, LogLevelEnum.Error);
                return null;
            }
        }

        [HttpPost]
        [Route("{applicationLocaleId:int}/Reassign")]
        [ValidateAntiForgeryToken]
        public Task<UserApplicationLocale> Reassign(int applicationLocaleId, [FromBody] ReassignUserApplicationLocaleModel model)
        {
            var permissionData = CreatePermissionData();

            try
            {
                return ApplicationLocaleManager.ReassignApplicationLocaleAsync(permissionData, applicationLocaleId, model.AssignFromUserId, model.AssignToUserEmail);
            }
            catch (Exception ex)
            {
                Logapi.WriteToLog(ex.GetBaseException().Message, LogLevelEnum.Error);
                return null;
            }
        }

        [HttpPost]
        [Route("{applicationLocaleId:int}/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> Delete(int applicationLocaleId)
        {
            var permissionData = CreatePermissionData();

            try
            {
                await ApplicationLocaleManager.DeleteApplicationLocaleAsync(permissionData, applicationLocaleId);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logapi.WriteToLog(ex.GetBaseException().Message, LogLevelEnum.Error);
                return null;
            }
        }


        [HttpPost]
        [Route("OnboardApp")]
        [ValidateAntiForgeryToken]
        public Task<int> OnboardApp([FromBody] CreateUserApplicationModel model)
        {
            var permissionData = CreatePermissionData();

            try
            {
                return ApplicationLocaleManager.ApplicationOnboarding( model);
            }
            catch (Exception ex)

            {
                Logapi.WriteToLog(ex.GetBaseException().Message, LogLevelEnum.Error);
                return null;
            }
        }

        [HttpPost]
        [Route("{applicationLocaleId:int}/AddAppManager")]
        [ValidateAntiForgeryToken]
        public Task<int> AddAppManager(int applicationLocaleId, [FromBody] ReassignUserApplicationLocaleModel model)
        {
            var permissionData = CreatePermissionData();

            try
            {
                return ApplicationLocaleManager.AddAppManagerAsync(permissionData, applicationLocaleId, model.AssignToUserEmail);
            }
            catch (Exception ex)
            {
                Logapi.WriteToLog(ex.GetBaseException().Message, LogLevelEnum.Error);
                return null;
            }
        }

    }
}



