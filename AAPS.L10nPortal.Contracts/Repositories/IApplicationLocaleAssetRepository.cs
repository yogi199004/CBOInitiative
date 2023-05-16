using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Contracts.Repositories
{
    public interface IApplicationLocaleAssetRepository
    {
        IEnumerable<ApplicationLocaleAsset> GetList(PermissionData permissionData, int applicationLocaleId);
        IEnumerable<ApplicationLocaleAsset> GetAssetKeyWithAsset(PermissionData permissionData, int applicationLocaleId);
        IEnumerable<ApplicationLocaleAsset> Update(PermissionData permissionData, int applicationLocaleId, int keyId, string value);
        IEnumerable<ApplicationLocaleAsset> Get(PermissionData permissionData, int applicationLocaleId, int keyId);
    }
}

