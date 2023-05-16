namespace AAPS.L10nPortal.Entities
{
    public class ApplicationLocaleResourceKeyValue
    {
        public int ApplicationId { get; set; }

        public int LocaleId { get; set; }

        public string LocaleCode { get; set; }

        public string ResourceKey { get; set; }

        public string ResourceKeyTypeId { get; set; }

        public string ResourceValue { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
