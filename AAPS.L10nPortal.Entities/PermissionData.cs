namespace CAPPortal.Entities
{
    public class PermissionData : PrincipalData
    {
        public PermissionData(PrincipalData principalData, Guid userId) : base(principalData.UserEmail)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}
