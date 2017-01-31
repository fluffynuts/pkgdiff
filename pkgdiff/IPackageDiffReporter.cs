namespace pkgdiff
{
    public interface IPackageDiffReporter
    {
        IConsoleMessage Translate(IPackageDifference diff);
    }
}