using AAPS.L10nPortal.Contracts.Models;
using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Contracts.Managers
{
    public interface IApplicationLocaleAssetManager
    {
        Task<ApplicationAssets> GetListAsync(PermissionData permissionData, int applicationLocaleId);
        Task<ApplicationAssets> GetAssetKeysWithAssets(PermissionData permissionData, int applicationLocaleId);
        Task<DownloadAssetFileResult> Download(PermissionData permissionData, int applicationLocaleId, int keyId);
        Task<Asset> UploadAsync(PermissionData permissionData, int applicationLocaleId, int key, string fileName, Stream inputStream);
    }
}
