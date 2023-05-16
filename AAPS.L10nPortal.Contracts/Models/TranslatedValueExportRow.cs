using AAPS.L10nPortal.Contracts.Attributes;

namespace AAPS.L10nPortal.Contracts.Models
{
    [ExcelSheetName("Translation")]
    public class TranslatedValueExportRow
    {
        public bool AutoTranslated { get; set; }

        public int Row { get; set; }

        public int TypeId { get; set; }

        public Guid? UpdatedBy { get; set; }

        [ExcelColumnOrdering("Element ID", 1)]
        public string Key { get; set; }

        [ExcelColumnOrdering("en-US", 2)]
        public string OriginalValue { get; set; }

        [ExcelColumnOrdering("Translation", 3)]
        public string TranslatedValue { get; set; }
        [ExcelColumnOrdering("Description", 4)]
        public string Description { get; set; }

        [ExcelColumnOrdering("Last Modified User Email", 5, false)]
        public string UpdatedEmail { get; set; }

        [ExcelColumnOrdering("Last Modified User Name", 6, false)]
        public string UpdatedName { get; set; }

        [ExcelColumnOrdering("Last Modified Date", 7, false)]
        public DateTime? UpdatedDate { get; set; }

        [ExcelColumnOrdering("en-US Last Modified Date", 8, false)]
        public DateTime? OriginalUpdatedDate { get; set; }
    }
}
