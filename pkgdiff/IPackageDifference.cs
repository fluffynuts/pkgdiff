namespace pkgdiff
{
    public interface IPackageDifference
    {
        string PackageId { get; }
        string LeftVersion { get; }
        string RightVersion { get; }
    }
}