using CAPPortal.Contracts.Attributes;

namespace AAPS.L10nPortal.Contracts.Models
{
    [ExcelSheetName("en-US")]
    public class OriginalValueExportRow
    {
        public int Row { get; set; }

        public Guid? UpdatedBy { get; set; }

        public int TypeId { get; set; }

        [ExcelColumnOrdering("Element ID", 1)]
        public string Key { get; set; }

        [ExcelColumnOrdering("en-US", 2)]
        public string OriginalValue { get; set; }

        [ExcelColumnOrdering("Description", 3)]
        public string Description { get; set; }

        [ExcelColumnOrdering("Last Modified User Email", 4, false)]
        public string UpdatedEmail { get; set; }

        [ExcelColumnOrdering("Last Modified User Name", 5, false)]
        public string UpdatedName { get; set; }

        [ExcelColumnOrdering("Last Modified Date", 6, false)]
        public DateTime? UpdatedDate { get; set; }
    }
}
