using System.IO;

namespace pkgdiff
{
    public class TextFileReader: ITextFileReader
    {
        public string Read(string path)
        {
            return File.ReadAllText(path);
        }
    }
}