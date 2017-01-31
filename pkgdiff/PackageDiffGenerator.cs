using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace pkgdiff
{
    public class PackageDiffGenerator: IPackageDiffGenerator
    {
        public IEnumerable<IPackageDifference> Diff(string leftXml, string rightXml)
        {
            var docLeft = XDocument.Parse(leftXml);
            var docRight = XDocument.Parse(rightXml);
            var xpath = "/packages/package";
            var leftPackages = docLeft.XPathSelectElements(xpath);
            var rightPackages = docRight.XPathSelectElements(xpath).ToArray();
            var allPackages = leftPackages.Union(rightPackages).Distinct(new CompareNodesByIdComparer());
            return allPackages.Select(lp =>
            {
                var packageId = lp.Attribute("id")?.Value;
                Func<XElement, bool> matcher = n => n.Attribute("id")?.Value == packageId;
                var leftMatch = leftPackages.FirstOrDefault(matcher);
                var rightMatch = rightPackages.FirstOrDefault(matcher);
                return GenerateDiffFor(packageId, leftMatch, rightMatch);
            })
            .Where(IsDifferent)
            .OrderBy(p => p.PackageId.ToLower());
        }

        private bool IsDifferent(IPackageDifference arg)
        {
            return arg.LeftVersion != arg.RightVersion;
        }

        private IPackageDifference GenerateDiffFor(string packageId, XElement leftNode, XElement rightNode)
        {
            return new PackageDifference(
                packageId,
                leftNode?.Attribute("version")?.Value,
                rightNode?.Attribute("version")?.Value
            );
        }
    }
}
