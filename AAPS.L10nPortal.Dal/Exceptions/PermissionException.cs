namespace CAPPortal.Dal.Exceptions
{
    [Serializable]
    public class PermissionException : Exception
    {
        public PermissionException() : base("You are not authorized to work with this Application and/or Locale or item was not found.")
        {
        }
    }
}
