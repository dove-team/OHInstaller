using Avalonia;
using Avalonia.Extensions.Controls;

namespace OHInstaller
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppBuilder.Configure<App>()
                    .LogToTrace()
                    .UsePlatformDetect()
                   .UseDoveExtensions()
                   //.UseChineseInputSupport()
            .StartWithClassicDesktopLifetime(args);
        }
    }
}