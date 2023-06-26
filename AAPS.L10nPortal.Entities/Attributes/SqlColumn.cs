namespace CAPPortal.Entities.Attributes
{
    public abstract class SqlColumn : Attribute
    {
        public int Order { get; private set; }

        protected SqlColumn(int order)
        {
            Order = order;
        }

        public abstract string GetSqlType();

        public abstract string GetSqlValue(object value);
    }
}
