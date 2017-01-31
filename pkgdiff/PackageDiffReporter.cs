using System;
using System.Linq;

namespace pkgdiff
{
    public interface IConsoleMessage
    {
        string Message { get; }
        ConsoleColor Color { get; }
    }

    public class ConsoleMessage: IConsoleMessage
    {
        public string Message { get; }
        public ConsoleColor Color { get; }

        public ConsoleMessage(string message, ConsoleColor color)
        {
            Message = message;
            Color = color;
        }
    }

    public class PackageDiffReporter: IPackageDiffReporter
    {
        private readonly Func<IPackageDifference, IConsoleMessage>[] _strategies =
        {
            d => d.PackageId == null ? ErrorMessage("No package id") : null,
            d => d.RightVersion == null && d.LeftVersion == null ? ErrorMessage("Both package versions missing") : null,
            d => d.LeftVersion == null ? ConsoleMessage($"Right only: {d.PackageId}: {d.RightVersion}", ConsoleColor.Cyan): null,
            d => d.RightVersion == null ? ConsoleMessage($"Left only:  {d.PackageId}: {d.LeftVersion}", ConsoleColor.DarkYellow) : null,
            d => d.LeftVersion == d.RightVersion ? ConsoleMessage($"Unchanged:   {d.PackageId}", ConsoleColor.Gray) : null,
            d => LeftVersionOf(d) < RightVersionOf(d) ? UpDownMessageFor(d, "Upgraded:  ", ConsoleColor.Green) : null,
            d => LeftVersionOf(d) > RightVersionOf(d) ? UpDownMessageFor(d, "Downgraded:", ConsoleColor.Red) : null,
        };

        private static IConsoleMessage ErrorMessage(string subMessage)
        {
            return new ConsoleMessage($"Error: {subMessage}", ConsoleColor.DarkRed);
        }

        private static IConsoleMessage UpDownMessageFor(IPackageDifference d, string subMessage, ConsoleColor color)
        {
            return ConsoleMessage($"{subMessage} {d.PackageId}: {d.LeftVersion} => {d.RightVersion}", color);
        }
        private static Version RightVersionOf(IPackageDifference packageDifference)
        {
            return new Version(packageDifference.RightVersion);
        }

        private static Version LeftVersionOf(IPackageDifference packageDifference)
        {
            return new Version(packageDifference.LeftVersion);
        }

        private static IConsoleMessage ConsoleMessage(string message, ConsoleColor color)
        {
            return new ConsoleMessage(message, color);
        }

        public IConsoleMessage Translate(IPackageDifference diff)
        {
            return _strategies
                        .Select(s => s(diff))
                        .FirstOrDefault(s => s != null);
        }
    }

}