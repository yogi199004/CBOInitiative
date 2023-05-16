namespace AAPS.L10nPortal.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnAttribute : Attribute
    {
        public string ColumnName { get; }
        public ExcelColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
