using CAPPortal.Contracts.Models;
using CAPPortal.Entities;

namespace CAPPortal.Contracts.Repositories
{
    public interface IApplicationLocaleAssetRepository
    {
        Task <IEnumerable<Asset>> GetList(PermissionData permissionData, int applicationLocaleId);
        IEnumerable<ApplicationLocaleAsset> GetAssetKeyWithAsset(PermissionData permissionData, int applicationLocaleId);
        IEnumerable<ApplicationLocaleAsset> Update(PermissionData permissionData, int applicationLocaleId, int keyId, string value);
        IEnumerable<ApplicationLocaleAsset> Get(PermissionData permissionData, int applicationLocaleId, int keyId);
    }
}

