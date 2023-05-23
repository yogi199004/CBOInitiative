using AAPS.L10nPortal.Contracts.Managers;
using AAPS.L10nPortal.Contracts.Models;
using AAPS.L10nPortal.Contracts.Services;
using AAPS.L10nPortal.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.L10nPortal.Web.Controllers.WebApi
{
    [Route("api/Filter")]
    public class FilterController : BaseApiController
    {
        private IApplicationLocaleManager ApplicationLocaleManager { get; }

        private IApplicationLocaleAssetManager ApplicationLocaleAssetManager { get; }

        private ILogger<FilterController> Logger { get; }
        public FilterController(ILogger<FilterController> _log, IPermissionDataService permissionDataService, IApplicationLocaleManager appLocaleManager, IApplicationLocaleAssetManager appLocaleAssetManager) : base(permissionDataService)
        {
            this.ApplicationLocaleManager = appLocaleManager;
            this.ApplicationLocaleAssetManager = appLocaleAssetManager;
            Logger = _log;

        }

        [HttpPost]
        [Route("GetDashboardFilterData")]
        public async Task<IEnumerable<UserApplicationLocale>> GetDashboardFilterData([FromBody] DashboardFilterData data)
        {
            var permissionData = CreatePermissionData();
            try
            {
                var Locales = await this.ApplicationLocaleManager.GetUserApplicationLocaleListAsync();
                List<UserApplicationLocale> userApplicationLocale = new List<UserApplicationLocale>();

                if (data.IsFirstFilter)
                {
                    if (data.columnName.Equals("ApplicationName"))
                    {
                        return userApplicationLocale = Locales.Where(l =>
                data.ApplicationName.Any(d => l.ApplicationName.Equals(d))).ToList();
                    }
                    else if (data.columnName.Equals("PreferredName"))
                    {
                        return userApplicationLocale = Locales.Where(l =>
               data.PreferredName.Any(d => l.PreferredName.Equals(d))).ToList();
                    }
                    else if (data.columnName.Equals("LocaleCode"))
                    {
                        return userApplicationLocale = Locales.Where(l =>
               data.LocaleCode.Any(d => l.LocaleCode.Equals(d))).ToList();
                    }
                    else
                    {
                        return userApplicationLocale = Locales.Where(l =>
               data.UpdatedDate.Any(d3 => l.UpdatedDate.GetValueOrDefault().Year == d3.Year && l.UpdatedDate.GetValueOrDefault().Month == d3.Month && l.UpdatedDate.GetValueOrDefault().Day == d3.Day)).ToList();
                    }
                }
                else
                {
                    return userApplicationLocale = Locales.Where(l =>
                  data.ApplicationName.Any(d => l.ApplicationName.Equals(d))).Where(l1 => data.PreferredName.Any(d1 => l1.PreferredName.Equals(d1)))
                  .Where(l2 => data.LocaleCode.Any(d2 => l2.LocaleCode.Equals(d2)))
                  .Where(l3 => data.UpdatedDate.Any(d3 => l3.UpdatedDate.GetValueOrDefault().Year == d3.Year && l3.UpdatedDate.GetValueOrDefault().Month == d3.Month && l3.UpdatedDate.GetValueOrDefault().Day == d3.Day)).ToList();

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }



        [HttpPost]
        [Route("GetAssetFilterData")]
        public async Task<ApplicationAssets> GetAssetFilterData([FromBody] AssetFilterData data)
        {
            var permissionData = CreatePermissionData();
            try
            {
                var Assets = await this.ApplicationLocaleAssetManager.GetListAsync( data.ApplicationLocaleId);
                ApplicationAssets filteredAssets = new ApplicationAssets();

                filteredAssets.ApplicationName = Assets.ApplicationName;
                filteredAssets.LocaleCode = Assets.LocaleCode;

                if (data.IsFirstFilter)
                {
                    if (data.columnName.Equals("Key"))
                    {
                        filteredAssets.Assets = Assets.Assets.Where(l =>
                          data.Key.Any(d => l.Key.Equals(d))).ToList();
                        return filteredAssets;
                    }
                    else if (data.columnName.Equals("UpdatedDate"))
                    {
                        filteredAssets.Assets = Assets.Assets.Where(l =>
                        data.UpdatedDate.Any(d3 => l.UpdatedDate.GetValueOrDefault().Year == d3.Year && l.UpdatedDate.GetValueOrDefault().Month == d3.Month && l.UpdatedDate.GetValueOrDefault().Day == d3.Day)).ToList();
                        return filteredAssets;
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    filteredAssets.Assets = Assets.Assets.Where(l =>
                    data.Key.Any(d => l.Key.Equals(d)))
                    .Where(l1 => data.UpdatedDate.Any(d1 => l1.UpdatedDate.GetValueOrDefault().Year == d1.Year && l1.UpdatedDate.GetValueOrDefault().Month == d1.Month && l1.UpdatedDate.GetValueOrDefault().Day == d1.Day)).ToList();
                    return filteredAssets;
                }
            }

            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }

        [HttpGet]
        [Route("{applicationLocaleId:int}")]
        public async Task<ApplicationAssets> Get(int applicationLocaleId)
        {
            var permissionData = CreatePermissionData();
            try
            {
                return await this.ApplicationLocaleAssetManager.GetAssetKeysWithAssets(permissionData, applicationLocaleId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
