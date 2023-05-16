using AAPS.L10nPortal.Entities.Attributes;

namespace AAPS.L10nPortal.Entities
{
    public class ResourceKeyValue
    {
        [StringSqlColumn(1, 255)]
        public string Key { get; set; }

        [StringSqlColumn(2, int.MaxValue)]
        public string Value { get; set; }

        [IntegerSqlColumn(3)]
        public int TypeId { get; set; }

        [StringSqlColumn(4, int.MaxValue)]
        public string Description { get; set; }
    }
}
