using CAPPortal.Bal.Exceptions;
using CAPPortal.Bal.Extensions;
using CAPPortal.Contracts.Managers;
using CAPPortal.Contracts.Models;
using CAPPortal.Contracts.Services;
using CAPPortal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;


namespace CAPPortal.Web.Controllers.WebApi
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
        //[AllowAnonymous]
        public async Task<IEnumerable<Asset>> Get( int applicationLocaleId)
        {
            var permissionData = CreatePermissionData();
            try
            {
                return await this.ApplicationLocaleAssetManager.GetListAsync(permissionData,applicationLocaleId);
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
