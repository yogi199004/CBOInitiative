namespace CAPPortal.Dal.Exceptions
{
    [Serializable]
    public class OmniaRedisInstanceFulledException : CustomSqlException
    {
        public OmniaRedisInstanceFulledException() : base("No redis available for Omnia.")
        {
        }
    }
}