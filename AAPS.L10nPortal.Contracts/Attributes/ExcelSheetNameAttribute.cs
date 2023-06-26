namespace CAPPortal.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ExcelSheetNameAttribute : Attribute
    {
        public string SheetName { get; }

        public ExcelSheetNameAttribute(string sheetName)
        {
            SheetName = sheetName;
        }
    }
}
