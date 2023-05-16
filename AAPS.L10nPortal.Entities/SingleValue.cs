namespace AAPS.L10nPortal.Entities
{
    public class SingleValue<T> where T : IConvertible
    {
        public T Value { get; set; }
    }
}
