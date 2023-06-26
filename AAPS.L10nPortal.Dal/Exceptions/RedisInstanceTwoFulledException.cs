namespace CAPPortal.Dal.Exceptions
{
    public class RedisInstanceTwoFulledException : CustomSqlException
    {
        public RedisInstanceTwoFulledException() : base("No Redis Instance available for Non Omnia.")
        {
        }
    }
}