using Avalonia.Controls;
using YpdfDesktop.Infrastructure.Search;

namespace YpdfDesktop.Infrastructure.Communication
{
    internal static class MainWindowMessage
    {
        public static void ShowErrorDialog(string message)
        {
            var quickMessage = new QuickMessage(message);

            if (WindowFinder.FindMainWindow() is Window window)
                quickMessage.ShowErrorDialog(window);
            else
                quickMessage.Show();
        }

        public static void ShowWarningDialog(string message)
        {
            var quickMessage = new QuickMessage(message);

            if (WindowFinder.FindMainWindow() is Window window)
                quickMessage.ShowWarningDialog(window);
            else
                quickMessage.Show();
        }

        public static void ShowInformationDialog(string message)
        {
            var quickMessage = new QuickMessage(message);

            if (WindowFinder.FindMainWindow() is Window window)
                quickMessage.ShowInformationDialog(window);
            else
                quickMessage.Show();
        }
    }
}
