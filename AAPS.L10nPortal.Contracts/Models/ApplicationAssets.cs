namespace CAPPortal.Contracts.Models
{
    public class ApplicationAssets
    {
        public string ApplicationName { get; set; }
        public string LocaleCode { get; set; }
        public IEnumerable<Asset> Assets { get; set; }
    }
}
