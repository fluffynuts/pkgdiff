using System;
using System.IO;

namespace pkgdiff
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            CheckValidityOf(args);
            var tool = new PackageDiffCommandlineTool(
                new TextFileReader(),
                new PackageDiffGenerator(),
                new PackageDiffReporter(),
                new ColorConsoleWriter()
            );
            tool.Diff(args[0], args[1]);
            return 0;
        }

        private static void CheckValidityOf(string[] args)
        {
            if (args.Length != 2) throw new ArgumentException("Please specify two package.config files to compare");
            foreach (var f in args)
                VerifyFileExists(f);
        }

        private static void VerifyFileExists(string s)
        {
            if (!File.Exists(s))
                throw new FileNotFoundException($"File not found: ${s}", s);
        }
    }
}
