using System.Collections.Generic;
using System.Xml.Linq;

namespace pkgdiff
{
    public class CompareNodesByIdComparer : IEqualityComparer<XElement>
    {
        public bool Equals(XElement x, XElement y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.Attribute("id")?.Value == y.Attribute("id")?.Value;
        }

        public int GetHashCode(XElement obj)
        {
            return obj.Attribute("id")?.Value?.GetHashCode() ?? 0;
        }
    }
}