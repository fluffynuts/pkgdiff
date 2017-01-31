namespace pkgdiff
{
    internal class PackageDifference: IPackageDifference
    {
        public string PackageId { get; }
        public string LeftVersion { get; }
        public string RightVersion { get; }

        public PackageDifference(string packageName, string leftVersion, string rightVersion)
        {
            PackageId = packageName;
            LeftVersion = leftVersion;
            RightVersion = rightVersion;
        }
    }
}