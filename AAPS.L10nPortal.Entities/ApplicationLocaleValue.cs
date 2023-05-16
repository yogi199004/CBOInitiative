using AAPS.L10nPortal.Entities.Attributes;

namespace AAPS.L10nPortal.Entities
{
    public class ApplicationLocaleValue
    {
        public int KeyId { get; set; }

        [StringSqlColumn(1, 255)]
        public string Key { get; set; }

        public string OriginalValue { get; set; }

        [StringSqlColumn(2, int.MaxValue)]
        public string TranslatedValue { get; set; }

        public string Description { get; set; }

        public int TypeId { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? OriginalUpdatedDate { get; set; }
    }
}
