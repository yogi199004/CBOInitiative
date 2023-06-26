namespace CAPPortal.Contracts.Models
{
    public class Asset
    {
        public int ApplicationLocaleId { get; set; }
        public int KeyId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool Changed { get; set; }
        public DateTime? OriginalUpdatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
