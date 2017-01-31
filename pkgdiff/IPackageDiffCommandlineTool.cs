namespace pkgdiff
{
    public interface IPackageDiffCommandlineTool
    {
        void Diff(string leftFilePath, string rightFilePath);
    }
}