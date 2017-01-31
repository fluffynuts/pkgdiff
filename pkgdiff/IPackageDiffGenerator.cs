using System.Collections.Generic;

namespace pkgdiff
{
    public interface IPackageDiffGenerator
    {
        IEnumerable<IPackageDifference> Diff(string leftXml, string rightXml);
    }
}