namespace CAPPortal.Entities
{
    public class ApplicationLocaleAsset : Updatable
    {
        public int ApplicationLocaleId { get; set; }
        public int KeyId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? OriginalUpdatedDate { get; set; }
    }
}
