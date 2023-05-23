using AAPS.L10nPortal.Bal.Exceptions;
using AAPS.L10nPortal.Bal.Extensions;
using AAPS.L10nPortal.Contracts.Managers;
using AAPS.L10nPortal.Contracts.Models;
using AAPS.L10nPortal.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;


namespace AAPS.L10nPortal.Web.Controllers.WebApi
{  
    [Route("api/ApplicationLocaleAsset")]
    public class ApplicationLocaleAssetController : BaseApiController
    {
        private IApplicationLocaleAssetManager ApplicationLocaleAssetManager { get; }
        private ILogger<ApplicationLocaleAssetController> Logger { get; }

        public ApplicationLocaleAssetController(ILogger<ApplicationLocaleAssetController> _log, IPermissionDataService permissionDataService, IApplicationLocaleAssetManager applicationLocaleAssetManager) : base(permissionDataService)
        {
            this.ApplicationLocaleAssetManager = applicationLocaleAssetManager;
            Logger = _log;
        }
        
        [HttpGet]
        [Route("{applicationLocaleId:int}")]
        [AllowAnonymous]
        public async Task<ApplicationAssets> Get(int applicationLocaleId)
        {
            
            try
            {
                return await this.ApplicationLocaleAssetManager.GetListAsync(applicationLocaleId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }

        [HttpGet]
        [Route("Download/{applicationLocaleId:int}/{keyId:int}")]
        public async Task<FileStreamResult> ExportToExcel(int applicationLocaleId, int keyId)
        {
            var permissionData = CreatePermissionData();
            try
            {
                var result = await ApplicationLocaleAssetManager.Download(permissionData, applicationLocaleId, keyId);

                string contentType = "";
                new FileExtensionContentTypeProvider().TryGetContentType(result.FileName, out contentType);


                return new FileStreamResult(result.FileContent, contentType)
                {
                    FileDownloadName = result.FileName
                };

                //return result.FileContent.StreamToHttpResponse(result.FileName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Upload/{applicationLocaleId:int}/{keyId:int}")]
        public Task<Asset> Post(int applicationLocaleId, int keyId)
        {
            var request = HttpContext.Request;

            if (request.Form.Files.Count != 1)
                throw new BadRequestException("Multiple files are not supported. Upload only 1 file.");

            if (request.Form.Files[0].FileName.IsFileExtensionBlacklisted())
                throw new BadRequestException("Uploaded file type is not allowed");

            var permissionData = CreatePermissionData();

            try
            {
                var uploadedFile = request.Form.Files[0];

                return ApplicationLocaleAssetManager.UploadAsync(permissionData, applicationLocaleId, keyId,
                    uploadedFile.FileName, uploadedFile.OpenReadStream());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
