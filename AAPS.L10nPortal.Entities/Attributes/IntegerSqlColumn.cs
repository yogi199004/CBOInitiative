namespace AAPS.L10nPortal.Entities.Attributes
{
    public class IntegerSqlColumn : SqlColumn
    {
        public IntegerSqlColumn(int order) : base(order)
        {
        }

        public override string GetSqlType()
        {
            return $"INT";
        }

        public override string GetSqlValue(object value)
        {
            return value.ToString();
        }
    }
}
