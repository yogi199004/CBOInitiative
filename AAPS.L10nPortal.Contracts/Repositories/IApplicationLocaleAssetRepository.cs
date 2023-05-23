using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Contracts.Repositories
{
    public interface IApplicationLocaleAssetRepository
    {
        Task <IEnumerable<ApplicationLocaleAsset>> GetList(int applicationLocaleId);
        IEnumerable<ApplicationLocaleAsset> GetAssetKeyWithAsset(PermissionData permissionData, int applicationLocaleId);
        IEnumerable<ApplicationLocaleAsset> Update(PermissionData permissionData, int applicationLocaleId, int keyId, string value);
        IEnumerable<ApplicationLocaleAsset> Get(PermissionData permissionData, int applicationLocaleId, int keyId);
    }
}

