using CAPPortal.Contracts.Models;
using CAPPortal.Entities;

namespace CAPPortal.Contracts.Managers
{
    public interface IApplicationLocaleAssetManager
    {
        Task<ApplicationAssets> GetListAsync( int applicationLocaleId);
        Task<ApplicationAssets> GetAssetKeysWithAssets(PermissionData permissionData, int applicationLocaleId);
        Task<DownloadAssetFileResult> Download(PermissionData permissionData, int applicationLocaleId, int keyId);
        Task<Asset> UploadAsync(PermissionData permissionData, int applicationLocaleId, int key, string fileName, Stream inputStream);
    }
}
