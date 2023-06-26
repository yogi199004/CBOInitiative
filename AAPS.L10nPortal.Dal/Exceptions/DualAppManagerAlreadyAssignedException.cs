﻿namespace CAPPortal.Dal.Exceptions
{
    [Serializable]
    class DualAppManagerAlreadyAssignedException : CustomSqlException
    {
        public DualAppManagerAlreadyAssignedException() : base("Dual App Manager Already Assigned.")
        {
        }
    }
}
