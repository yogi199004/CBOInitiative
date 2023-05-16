using L10N.API.Common;
using L10N.API.Contracts;
using L10N.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace L10n.Web.API.Controllers
{
    [Authorize(Roles = "L10NGlobalConsumers")]
    [Route("api/[controller]")]
    [ApiController]
    public class L10NController : BaseController
    {


        private ICosmosService cosmosService { get; }

        private IAPIService apiService { get; }

        private readonly ILogger<L10NController> log;

        public L10NController(ICosmosService _cosmosService, IAPIService _apiService, ILogger<L10NController> _log) : base(_log)
        {
            cosmosService = _cosmosService;
            apiService = _apiService;
            log = _log;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ObjectResult> GetMetaData(string appName)
        {
            try
            {

                if (String.IsNullOrWhiteSpace(appName))
                {
                    return BadRequest("Mandatory input parameters are missing");
                }
                else
                {
                    List<IEnumerable<MetaDataModel>> metaData;
                    //logic to establish connection with cosmos
                    var _cosmosClient = cosmosService.GetConnection();
                    var _cosmosContainerClient = cosmosService.getContainer(_cosmosClient, appName);
                    metaData = await apiService.getAppMetaData(_cosmosContainerClient, appName);

                    if (metaData.Count() < 1)
                    {
                        return BadRequest("No metadata available for the application " + appName);
                    }
                    else
                    {
                        return Ok(metaData.ToArray()[0]);
                    }
                }

            }
            catch (Exception ex)
            {
                var errorId = HandleException(ex, appName, nameof(GetMetaData));
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please try again or contact the administrator. Error ID: " + errorId);
            }

        }


        [Route("[action]")]
        [HttpGet]
        public async Task<ObjectResult> GetAllKeyValues(string appName, string locale)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(locale) || String.IsNullOrWhiteSpace(appName))
                {
                    return BadRequest("Mandatory input parameters are missing");
                }
                else
                {
                    List<IEnumerable<MetaDataModel>> metaData;


                    var _cosmosClient = cosmosService.GetConnection();
                    var _cosmosContainerClient = cosmosService.getContainer(_cosmosClient, appName);
                    metaData = await apiService.getAppMetaData(_cosmosContainerClient, appName);

                    String currentLocale = string.Empty;

                    if (metaData.Count() < 1)
                    {
                        return BadRequest("No metadata available for the application " + appName);
                    }
                    else
                    {
                        metaData.ForEach(x => { currentLocale = (x.Where(m => m.LocaleCode.Equals(locale, StringComparison.OrdinalIgnoreCase))?.FirstOrDefault()?.LocaleCode?.ToString() ?? null); });

                        if (currentLocale != null)
                        {
                            List<KeyValues> keyValuesData = new List<KeyValues>();
                            keyValuesData = await apiService.getAppKeyValuesData(_cosmosContainerClient, appName, locale);

                            if (keyValuesData.ToArray().Length < 1)
                            {
                                return BadRequest("There are no keys exists in the given locale " + locale);
                            }
                            else
                            {
                                return Ok(keyValuesData.ToArray()[0].AllKeyValues.keyValues);
                            }
                        }
                        else
                        {
                            return BadRequest("The locale " + locale + " is not a valid locale ");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                var errorId = HandleException(ex, appName, nameof(GetAllKeyValues));
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please try again or contact the administrator. Error ID: " + errorId);
            }

            //logic to fetch data and return 
        }


        [Route("[action]")]
        [HttpGet]
        public async Task<ObjectResult> GetValueByKey(string appName, string locale, string key)
        {

            try
            {
                List<IEnumerable<MetaDataModel>> metaData;

                var _cosmosClient = cosmosService.GetConnection();
                var _cosmosContainerClient = cosmosService.getContainer(_cosmosClient, appName);
                metaData = await apiService.getAppMetaData(_cosmosContainerClient, appName);

                String currentLocale = string.Empty;

                if (metaData.Count() < 1)
                {
                    return BadRequest("No metadata available for the application " + appName);
                }
                else
                {
                    metaData.ForEach(x => { currentLocale = (x.Where(m => m.LocaleCode.Equals(locale, StringComparison.OrdinalIgnoreCase))?.FirstOrDefault()?.LocaleCode?.ToString() ?? null); });

                    if (currentLocale != null)
                    {
                        List<KeyValues> keyValuesData = new List<KeyValues>();
                        keyValuesData = await apiService.getAppKeyValuesData(_cosmosContainerClient, appName, locale);

                        if (keyValuesData.ToArray().Length > 0 && keyValuesData.ToArray()[0].AllKeyValues.keyValues.ContainsKey(key))
                        {
                            return Ok(JsonConvert.SerializeObject(keyValuesData.ToArray()[0].AllKeyValues.keyValues[key]));
                        }
                        else
                        {
                            return BadRequest("The key " + key + " or value does not exists in the locale " + locale);
                        }
                    }
                    else
                    {
                        return BadRequest("The locale " + locale + " is not a valid locale ");
                    }

                }

            }
            catch (Exception ex)
            {
                var errorId = HandleException(ex, appName, nameof(GetValueByKey));
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please try again or contact the administrator. Error ID: " + errorId);
            }

        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ObjectResult> GetValuesByKeyList([FromBody] KeyList data)
        {

            Dictionary<string, string> result = new Dictionary<string, string>();
            Dictionary<string, string> allKeyValues = new Dictionary<string, string>();
            try
            {

                if (string.IsNullOrEmpty(data.AppName) || string.IsNullOrEmpty(data.locale) || data.keys == null || data.keys.Length == 0)
                {
                    return BadRequest("Mandatory input parameters are missing");
                }
                else
                {
                    List<IEnumerable<MetaDataModel>> metaData;
                    //logic to establish connection with cosmos
                    var _cosmosClient = cosmosService.GetConnection();
                    var _cosmosContainerClient = cosmosService.getContainer(_cosmosClient, data.AppName);
                    metaData = await apiService.getAppMetaData(_cosmosContainerClient, data.AppName);

                    String currentLocale = string.Empty;

                    if (metaData.Count() < 1)
                    {
                        return BadRequest("No metadata available for the application " + data.AppName);
                    }
                    else
                    {
                        metaData.ForEach(x => { currentLocale = (x.Where(m => m.LocaleCode.Equals(data.locale, StringComparison.OrdinalIgnoreCase))?.FirstOrDefault()?.LocaleCode?.ToString() ?? null); });

                        if (currentLocale != null)
                        {
                            List<KeyValues> keyValuesData = new List<KeyValues>();
                            keyValuesData = await apiService.getAppKeyValuesData(_cosmosContainerClient, data.AppName, data.locale);

                            if (keyValuesData.ToArray().Length < 1)
                            {
                                foreach (string s in data.keys)
                                {
                                    result.Add(s, null);
                                }
                                return Ok(result);
                            }
                            else
                            {
                                allKeyValues = keyValuesData.ToArray()[0].AllKeyValues.keyValues;
                            }

                            foreach (string s in data.keys)
                            {
                                if (allKeyValues.ContainsKey(s))
                                {
                                    result.Add(s, (allKeyValues.GetValueOrDefault(s)));
                                }
                                else
                                {
                                    result.Add(s, null);
                                }

                            }
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest("The locale " + data.locale + " is not a valid locale ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorId = HandleException(ex, data.AppName, nameof(GetValuesByKeyList));
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please try again or contact the administrator. Error ID: " + errorId);
            }

        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ObjectResult> GetValuesByKeyPattern([FromBody] KeyModel data)
        {
            try
            {
                if (string.IsNullOrEmpty(data.AppName) || string.IsNullOrEmpty(data.Locale) || data.KeysPattern == null || data.KeysPattern.Length == 0)
                {
                    return BadRequest("Mandatory input parameters are missing");
                }
                else
                {
                    Dictionary<string, string> result = new Dictionary<string, string>();
                    Dictionary<string, string> allKeyValues = new Dictionary<string, string>();

                    List<IEnumerable<MetaDataModel>> metaData;

                    var _cosmosClient = cosmosService.GetConnection(); //logic to establish connection with cosmos
                    var _cosmosContainerClient = cosmosService.getContainer(_cosmosClient, data.AppName);
                    metaData = await apiService.getAppMetaData(_cosmosContainerClient, data.AppName);


                    String currentLocale = string.Empty;

                    if (metaData.Count() < 1)
                    {
                        return BadRequest("No metadata available for the application " + data.AppName);
                    }
                    else
                    {
                        metaData.ForEach(x => { currentLocale = (x.Where(m => m.LocaleCode.Equals(data.Locale, StringComparison.OrdinalIgnoreCase))?.FirstOrDefault()?.LocaleCode?.ToString() ?? null); });

                        if (currentLocale != null)
                        {
                            List<KeyValues> keyValuesData = new List<KeyValues>();
                            keyValuesData = await apiService.getAppKeyValuesData(_cosmosContainerClient, data.AppName, data.Locale);

                            if (keyValuesData.ToArray().Length < 1)
                            {
                                return BadRequest(String.Format("There are no keys exists in the given locale {0}", data.Locale));
                            }

                            else
                            {
                                allKeyValues = keyValuesData.ToArray()[0].AllKeyValues.keyValues;
                            }

                            foreach (string s in data.KeysPattern)
                            {
                                foreach (string key in allKeyValues.Keys)
                                {
                                    if ((!result.ContainsKey(key)) && key.StartsWith(s.Replace("*", String.Empty)))
                                    {
                                        result.Add(key, allKeyValues.GetValueOrDefault(key));
                                    }
                                }
                            }

                            if (result.Count == 0)
                            {
                                return BadRequest("There are no keys exists in the given locale " + data.Locale);
                            }
                            else
                            {
                                return Ok(result);
                            }
                        }
                        else
                        {
                            return BadRequest("The locale " + data.Locale + " is not a valid locale ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorId = HandleException(ex, data.AppName, nameof(GetValuesByKeyPattern));
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please try again or contact the administrator. Error ID: " + errorId);
            }

        }


        [Route("[action]")]
        [HttpGet]
        public async Task<ObjectResult> GetAllKeys(string appName, string locale)
        {
            try
            {
                if (string.IsNullOrEmpty(appName) || string.IsNullOrEmpty(locale))
                {
                    return BadRequest("Mandatory input parameters are missing");
                }
                else
                {
                    Dictionary<string, string> result = new Dictionary<string, string>();
                    Dictionary<string, string> allKeyValues = new Dictionary<string, string>();
                    List<IEnumerable<MetaDataModel>> metaData;

                    var _cosmosClient = cosmosService.GetConnection();
                    //logic to establish connection with cosmos
                    var _cosmosContainerClient = cosmosService.getContainer(_cosmosClient, appName);
                    metaData = await apiService.getAppMetaData(_cosmosContainerClient, appName);
                    String currentLocale = string.Empty;

                    if (metaData.Count() < 1)
                    {
                        return BadRequest("No metadata available for the application " + appName);
                    }
                    else
                    {
                        metaData.ForEach(x => { currentLocale = (x.Where(m => m.LocaleCode.Equals(locale, StringComparison.OrdinalIgnoreCase))?.FirstOrDefault()?.LocaleCode?.ToString() ?? null); });

                        if (currentLocale != null)
                        {
                            List<KeyValues> keyValuesData = new List<KeyValues>();
                            keyValuesData = await apiService.getAppKeyValuesData(_cosmosContainerClient, appName, locale);

                            if (keyValuesData.ToArray().Length > 0)
                            {
                                allKeyValues = keyValuesData.ToArray()[0].AllKeyValues.keyValues;
                            }

                            return Ok(allKeyValues.Keys.ToArray());
                        }
                        else
                        {
                            return BadRequest("The locale " + locale + " is not a valid locale");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                var errorId = HandleException(ex, appName, nameof(GetAllKeys));
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please try again or contact the administrator. Error ID: " + errorId);
            }
        }



        [Route("[action]")]
        [HttpPost]
        public async Task<ObjectResult> GetAssetSasToken()

        {

            var bodyreader = new StreamReader(Request.Body);
            var AssetName = bodyreader.ReadToEndAsync().Result;
            try
            {


                if (string.IsNullOrEmpty(AssetName.ToString()))
                {
                    return null;
                }
                else
                {
                    return Ok(apiService.GetSASUrl(AssetName.ToString(), ConfigValues.SAKey));
                }
            }
            catch (Exception ex)
            {
                var errorId = HandleException(ex, AssetName.ToString(), nameof(GetAssetSasToken));
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please try again or contact the administrator. Error ID: " + errorId);
            }

        }
    }
}
