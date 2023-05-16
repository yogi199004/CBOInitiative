namespace AAPS.L10nPortal.Dal.Exceptions
{
    [Serializable]
    class DualAppManagerAlreadyAssignedException : CustomSqlException
    {
        public DualAppManagerAlreadyAssignedException() : base("Dual App Manager Already Assigned.")
        {
        }
    }
}
