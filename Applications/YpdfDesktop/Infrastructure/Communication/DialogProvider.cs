using Avalonia.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using YpdfDesktop.Infrastructure.Search;

namespace YpdfDesktop.Infrastructure.Communication
{
    internal static class DialogProvider
    {
        private static readonly FileDialogFilter _pdfFilder = new()
        {
            Name = "PDF Documents",
            Extensions = new List<string>() { "pdf" }
        };

        public static Task<string[]?> GetPdfFilePaths(bool allowMultiple = false)
        {
            string title = "Select PDF file";

            if (allowMultiple)
                title += "s";

            var dialog = new OpenFileDialog()
            {
                Title = title,
                AllowMultiple = allowMultiple,
                Filters = new List<FileDialogFilter>() { _pdfFilder }
            };

            return WindowFinder.FindMainWindow() is Window mainWindow
                ? dialog.ShowAsync(mainWindow)
                : new Task<string[]?>(() => null);
        }

        public static Task<string?> GetOutputFilePath(string? initialFileName = null, bool isPdfFileOnly = false)
        {
            var dialog = new SaveFileDialog()
            {
                Title = "Select output file path",
                InitialFileName = initialFileName
            };

            if (isPdfFileOnly)
                dialog.Filters = new List<FileDialogFilter>() { _pdfFilder };

            return WindowFinder.FindMainWindow() is Window mainWindow
                ? dialog.ShowAsync(mainWindow)
                : new Task<string?>(() => null);
        }

        public static Task<string?> GetDirectoryPath()
        {
            var dialog = new OpenFolderDialog()
            {
                Title = "Select output directory path"
            };

            return WindowFinder.FindMainWindow() is Window mainWindow
                ? dialog.ShowAsync(mainWindow)
                : new Task<string?>(() => null);
        }
    }
}
