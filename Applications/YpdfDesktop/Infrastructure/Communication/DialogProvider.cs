using Avalonia.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using YpdfDesktop.Infrastructure.Search;

namespace YpdfDesktop.Infrastructure.Communication
{
    internal static class DialogProvider
    {
        private static readonly FileDialogFilter _defaultFilter = new()
        {
            Name = "All files",
            Extensions = new List<string>() { "*" }
        };

        private static readonly FileDialogFilter _textFilter = new()
        {
            Name = "Text files",
            Extensions = new List<string>() { "txt" }
        };

        private static readonly FileDialogFilter _jpgFilter = new()
        {
            Name = "JPG Images",
            Extensions = new List<string>() { "jpg", "jpeg" }
        };

        private static readonly FileDialogFilter _pngFilter = new()
        {
            Name = "PNG Images",
            Extensions = new List<string>() { "png" }
        };

        private static readonly FileDialogFilter _imagesFilter = new()
        {
            Name = "Images",
            Extensions = new List<string>() { "jpg", "jpeg", "png", "bmp", "gif", "tiff" }
        };
        
        private static readonly FileDialogFilter _pdfFilter = new()
        {
            Name = "PDF Documents",
            Extensions = new List<string>() { "pdf" }
        };

        public static IReadOnlyList<FileDialogFilter> DefaultFilters => new List<FileDialogFilter>() { _defaultFilter };
        public static IReadOnlyList<FileDialogFilter> TextFilters => new List<FileDialogFilter>() { _textFilter };
        public static IReadOnlyList<FileDialogFilter> ImageFilters => new List<FileDialogFilter>() { _jpgFilter, _pngFilter, _imagesFilter };
        public static IReadOnlyList<FileDialogFilter> PdfFilters => new List<FileDialogFilter>() { _pdfFilter };

        public static Task<string[]?> GetInputFilePaths(bool allowMultiple = false, IEnumerable<FileDialogFilter>? filters = null)
        {
            string title = "Select input file";

            if (allowMultiple)
                title += "s";

            var dialog = new OpenFileDialog()
            {
                Title = title,
                AllowMultiple = allowMultiple
            };

            if (filters is not null)
                dialog.Filters = new List<FileDialogFilter>(filters);

            return WindowFinder.FindMainWindow() is Window mainWindow
                ? dialog.ShowAsync(mainWindow)
                : new Task<string[]?>(() => null);
        }

        public static Task<string[]?> GetPdfFilePaths(bool allowMultiple = false)
        {
            return GetInputFilePaths(allowMultiple, new FileDialogFilter[] { _pdfFilter });
        }

        public static Task<string[]?> GetTxtFilePaths(bool allowMultiple = false)
        {
            return GetInputFilePaths(allowMultiple, new FileDialogFilter[] { _textFilter });
        }

        public static Task<string[]?> GetImageFilePaths(bool allowMultiple = false)
        {
            return GetInputFilePaths(allowMultiple, new FileDialogFilter[] { _imagesFilter });
        }

        public static Task<string?> GetOutputFilePath(string? initialFileName = null, IEnumerable<FileDialogFilter>? filters = null)
        {
            var dialog = new SaveFileDialog()
            {
                Title = "Select output file path",
                InitialFileName = initialFileName
            };

            if (filters is not null)
                dialog.Filters = new List<FileDialogFilter>(filters);

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
