using System.Reflection;

namespace CAPPortal.Entities.Attributes
{
    public class XmlProperty
    {
        public PropertyInfo PropertyInfo { get; set; }

        public SqlColumn SqlColumn { get; set; }
    }
}
