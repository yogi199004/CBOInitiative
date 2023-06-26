namespace CAPPortal.Entities.Attributes
{
    public sealed class StringSqlColumn : SqlColumn
    {
        private readonly int _length;

        public StringSqlColumn(int order, int length) : base(order)
        {
            _length = length;
        }

        public int Length => _length;

        public override string GetSqlType()
        {
            return $"NVARCHAR({_length})";
        }

        public override string GetSqlValue(object value)
        {
            return value.ToString();
        }
    }
}
