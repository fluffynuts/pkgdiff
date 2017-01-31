using System;

namespace pkgdiff
{
    public interface IPackageDiffItemWriter
    {
        void Write(ConsoleColor textColor, string text);
    }
}