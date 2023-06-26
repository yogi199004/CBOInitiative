namespace CAPPortal.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnOrderingAttribute : ExcelColumnAttribute
    {
        public int OrderNum { get; }

        public bool Required { get; }

        public ExcelColumnOrderingAttribute(string columnName, int orderNum, bool required = true) : base(columnName)
        {
            OrderNum = orderNum;
            Required = required;
        }
    }
}
