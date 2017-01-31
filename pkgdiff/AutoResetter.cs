using System;

namespace pkgdiff
{
    internal class AutoResetter: IDisposable
    {
        private readonly ConsoleColor _originalColor;

        public AutoResetter()
        {
            _originalColor = Console.ForegroundColor;
        }

        public void Dispose()
        {
            try
            {
                Console.ForegroundColor = _originalColor;
            }
            catch
            {
                /* do nothing */
            }
        }
    }
}