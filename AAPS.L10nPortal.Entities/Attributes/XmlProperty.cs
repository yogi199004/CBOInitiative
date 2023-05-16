using System.Reflection;

namespace AAPS.L10nPortal.Entities.Attributes
{
    public class XmlProperty
    {
        public PropertyInfo PropertyInfo { get; set; }

        public SqlColumn SqlColumn { get; set; }
    }
}
