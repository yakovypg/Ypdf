using Avalonia;
using Avalonia.ReactiveUI;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using System;

namespace YpdfDesktop
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder
                .Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI()
                .WithIcons(c => c.Register<FontAwesomeIconProvider>());
        }
    }
}
