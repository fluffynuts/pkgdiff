using System;

namespace pkgdiff
{
    public class ColorConsoleWriter: IPackageDiffItemWriter
    {
        public void Write(ConsoleColor textColor, string text)
        {
            using (new AutoResetter())
            {
                Console.ForegroundColor = textColor;
                Console.WriteLine(text);
            }
        }
    }
}