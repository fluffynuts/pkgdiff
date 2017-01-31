using System.Linq;

namespace pkgdiff
{
    public class PackageDiffCommandlineTool: IPackageDiffCommandlineTool
    {
        private readonly ITextFileReader _reader;
        private readonly IPackageDiffGenerator _generator;
        private readonly IPackageDiffReporter _reporter;
        private readonly IPackageDiffItemWriter _writer;

        public PackageDiffCommandlineTool(
            ITextFileReader reader,
            IPackageDiffGenerator generator,
            IPackageDiffReporter reporter, 
            IPackageDiffItemWriter writer
        )
        {
            _reader = reader;
            _generator = generator;
            _reporter = reporter;
            _writer = writer;
        }

        public void Diff(string leftFilePath, string rightFilePath)
        {
            var leftContents = _reader.Read(leftFilePath);
            var rightContents = _reader.Read(rightFilePath);
            var differences = _generator.Diff(leftContents, rightContents);
            var translated = differences.Select(_reporter.Translate);
            foreach (var line in translated)
                _writer.Write(line.Color, line.Message);
        }
    }
}