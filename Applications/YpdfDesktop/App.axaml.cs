using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using YpdfDesktop.ViewModels;
using YpdfDesktop.Views;

namespace YpdfDesktop
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // using Avalonia.Data.Core;
            // using Avalonia.Data.Core.Plugins;
            // ExpressionObserver.DataValidators.RemoveAll(t => t is DataAnnotationsValidationPlugin);
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
