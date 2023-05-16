namespace AAPS.L10nPortal.Entities
{
    public class UserApplicationLocale : ApplicationLocale
    {
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public string PreferredName { get; set; }
        public bool CanEdit { get; set; }
    }
}
