using Avalonia;
using Avalonia.Threading;
using ExecutionLib.Configuration;
using ExecutionLib.Informing.Aliases;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Infrastructure.Collections;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Infrastructure.Default;
using YpdfDesktop.Models;
using YpdfDesktop.Models.IO;
using YpdfDesktop.ViewModels.Base;
using YpdfDesktop.Models.Paging;
using YpdfLib.Models.Design;
using YpdfLib.Informing;
using YpdfLib.Models.Enumeration;
using YpdfLib.Models.Parsing;
using YpdfLib.Models.Paging;
using FileSystemLib.Naming;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class HandlePagesViewModel : PdfToolViewModel, IFilePathContainer, IPageCollectionContainer
    {
        #region Private Fields

        private int _sourcePdfFilePageCount;

        #endregion
        
        #region Commands

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectInputFilePathCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectOutputFilePathCommand { get; }
        public ReactiveCommand<string, Unit> ReorderPagesCommand { get; }
        public ReactiveCommand<string, Unit> TurnPagesLeftCommand { get; }
        public ReactiveCommand<string, Unit> TurnPagesRightCommand { get; }
        public ReactiveCommand<string, Unit> RemovePagesCommand { get; }
        public ReactiveCommand<int, Unit> TurnPageLeftCommand { get; }
        public ReactiveCommand<int, Unit> TurnPageRightCommand { get; }
        public ReactiveCommand<int, Unit> RemovePageCommand { get; }

        #endregion

        #region Public Properties

        public bool IsAnyPagesLeft => Pages.Count > 0;

        #endregion

        #region Reactive Properties

        private bool _isInputFilePathSelected = false;
        public bool IsInputFilePathSelected
        {
            get => _isInputFilePathSelected;
            private set => this.RaiseAndSetIfChanged(ref _isInputFilePathSelected, value);
        }

        private bool _isOutputFilePathSelected = false;
        public bool IsOutputFilePathSelected
        {
            get => _isOutputFilePathSelected;
            private set => this.RaiseAndSetIfChanged(ref _isOutputFilePathSelected, value);
        }

        private string _inputFilePath = string.Empty;
        public string InputFilePath
        {
            get => _inputFilePath;
            private set
            {
                this.RaiseAndSetIfChanged(ref _inputFilePath, value);
                IsInputFilePathSelected = !string.IsNullOrEmpty(value);
            }
        }

        private string _outputFilePath = string.Empty;
        public string OutputFilePath
        {
            get => _outputFilePath;
            private set
            {
                this.RaiseAndSetIfChanged(ref _outputFilePath, value);
                IsOutputFilePathSelected = !string.IsNullOrEmpty(value);
            }
        }

        private string _orderOfPages = string.Empty;
        public string OrderOfPages
        {
            get => _orderOfPages;
            private set => this.RaiseAndSetIfChanged(ref _orderOfPages, value);
        }

        #endregion

        #region Observable Collections

        public ExtendedObservableCollection<PageInfo> Pages { get; }
        ObservableCollection<PageInfo> IPageCollectionContainer.Pages => Pages;

        #endregion

        // Constructor for Designer
        public HandlePagesViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public HandlePagesViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            Pages = new ExtendedObservableCollection<PageInfo>();
            Pages.CollectionChanged += (s, e) => UpdatePagePositions();
            
            ExecuteCommand = ReactiveCommand.Create(Execute);
            ResetCommand = ReactiveCommand.Create(Reset);
            SelectInputFilePathCommand = ReactiveCommand.Create(SelectInputFilePath);
            SelectOutputFilePathCommand = ReactiveCommand.Create(SelectOutputFilePath);
            ReorderPagesCommand = ReactiveCommand.Create<string>(ReorderPages);
            TurnPagesLeftCommand = ReactiveCommand.Create<string>(TurnPagesLeft);
            TurnPagesRightCommand = ReactiveCommand.Create<string>(TurnPagesRight);
            RemovePagesCommand = ReactiveCommand.Create<string>(RemovePages);
            TurnPageLeftCommand = ReactiveCommand.Create<int>(TurnPageLeft);
            TurnPageRightCommand = ReactiveCommand.Create<int>(TurnPageRight);
            RemovePageCommand = ReactiveCommand.Create<int>(RemovePage);
        }

        #region Protected Methods

        protected override void Execute()
        {
            if (!VerifyOutputFilePath() || !VerifyPageCount())
                return;

            string outputPath = CorrectOutputFilePath(OutputFilePath, "pdf");

            YpdfConfig[] configs = new YpdfConfig[]
            {
                PrepareRemoveTool(),
                PrepareReorderTool(),
                PrepareRotateTool()
            };

            ExecuteManyAsOne(ToolType.HandlePages, configs, InputFilePath, outputPath, true);
        }

        protected override void Reset()
        {
            _sourcePdfFilePageCount = 0;

            Pages.Clear();
            
            InputFilePath = string.Empty;
            OutputFilePath = string.Empty;
            OrderOfPages = string.Empty;
        }

        #endregion

        #region Private Methods

        private YpdfConfig PrepareRemoveTool()
        {
            var allPages = Enumerable.Range(1, _sourcePdfFilePageCount);
            var currPages = Pages.Select(t => t.PageNumber);
            var pagesToRemove = allPages.Except(currPages);

            PageRange[] pageRangesToRemove = pagesToRemove
                .Select(t => new PageRange(t))
                .ToArray();
            
            var config = new YpdfConfig()
            {
                PdfTool = "remove-pages"
            };

            Array.ForEach(pageRangesToRemove, config.Pages.Add);

            return config;
        }

        private YpdfConfig PrepareReorderTool()
        {
            var orderedPages = Pages
                .OrderBy(t => t.PageNumber)
                .Select((t, i) => (Page: t, PageNumber: i + 1));

            int[] normalizedPageOrder = new int[Pages.Count];

            for (int i = 0; i < Pages.Count; ++i)
            {
                int normalizedPageNumber = orderedPages.First(t => t.Page == Pages[i]).PageNumber;
                normalizedPageOrder[i] = normalizedPageNumber;
            }

            var pageOrder = new PageOrder(normalizedPageOrder);
            
            var config = new YpdfConfig()
            {
                PdfTool = "reorder-pages",
                PageOrder = pageOrder
            };

            return config;
        }

        private YpdfConfig PrepareRotateTool()
        {
            PageRotation[] pageRotations = Pages
                .Select(t => new PageRotation(t.Position, t.RotationAngle))
                .ToArray();
            
            var config = new YpdfConfig()
            {
                PdfTool = "rotate"
            };

            Array.ForEach(pageRotations, config.PageRotations.Add);

            return config;
        }

        bool IFilePathContainer.SetFilePath(string path)
        {
            return SetInputFilePath(path);
        }

        private bool SetInputFilePath(string path)
        {
            if (!File.Exists(path))
                return false;

            if (!IsPathToPdf(path))
            {
                string message = $"{SettingsVM.Locale.FileNotPdfMessage}.";
                MainWindowMessage.ShowErrorDialog(message);
                return false;
            }

            if (!TryLoadPdfConfig(path))
            {
                string message = $"{SettingsVM.Locale.FailedToLoadFileMessage}.";
                MainWindowMessage.ShowErrorDialog(message);
                return false;
            }

            InputFilePath = path;
            return true;
        }

        private void SelectInputFilePath()
        {
            _ = DialogProvider.GetPdfFilePaths(false).ContinueWith(t =>
            {
                if (t.Result is null || t.Result.Length == 0)
                    return;

                Dispatcher.UIThread.Post(() => SetInputFilePath(t.Result[0]));
            });
        }

        private void SelectOutputFilePath()
        {
            const string initialFileName = "HandledPdf";

            _ = DialogProvider.GetOutputFilePath(initialFileName, DialogProvider.PdfFilters).ContinueWith(t =>
            {
                if (t.Result is null || string.IsNullOrEmpty(t.Result))
                    return;

                OutputFilePath = t.Result;
            });
        }

        private bool VerifyOutputFilePath()
        {
            return InformIfIncorrect(IsOutputFilePathSelected, SettingsVM.Locale.SpecifyOutputFilePathMessage);
        }

        private bool VerifyPageCount()
        {
            string? allPagedRemoved = SettingsVM.Locale.AllPagedRemovedMessage;
            string? operationCouldNotBePerformed = SettingsVM.Locale.OperationCouldNotBePerformedMessage;
            string message = $"{allPagedRemoved}. {operationCouldNotBePerformed}";

            return InformIfIncorrect(IsAnyPagesLeft, message);
        }

        private void ReorderPages(string pagesStr)
        {
            if (!TryParsePageRanges(pagesStr, out PageRange[] pageRanges))
            {
                string message = $"{SettingsVM.Locale.IncorrectPagesMessage}.";
                MainWindowMessage.ShowErrorDialog(message);
                return;
            }

            List<int> currPages = Pages.Select(t => t.PageNumber).ToList();

            IEnumerable<int> newOrder = PageRange.GetAllItems(pageRanges)
                .Where(t => currPages.Contains(t))
                .Distinct();

            if (newOrder.Count() < currPages.Count)
            {
                var remainingPages = currPages.Except(newOrder).OrderBy(t => t);
                newOrder = newOrder.Concat(remainingPages);
            }

            PageInfo[] orderedPages = newOrder
                .Select(n => Pages.First(t => t.PageNumber == n))
                .ToArray();
            
            Pages.ReplaceItems(orderedPages);
        }

        private void TurnPagesLeft(string pagesStr)
        {
            TurnPages(pagesStr, TurnPageLeft);
        }

        private void TurnPagesRight(string pagesStr)
        {
            TurnPages(pagesStr, TurnPageRight);
        }

        private void TurnPages(string pagesStr, Action<int> turnAction)
        {
            if (!TryParsePageRanges(pagesStr, out PageRange[] pageRanges))
            {
                string message = $"{SettingsVM.Locale.IncorrectPagesMessage}.";
                MainWindowMessage.ShowErrorDialog(message);
                return;
            }

            int[] pages = PageRange.GetAllItems(pageRanges);

            foreach (int page in pages)
            {
                turnAction?.Invoke(page);
            }
        }

        private void RemovePages(string pagesStr)
        {
            if (!TryParsePageRanges(pagesStr, out PageRange[] pageRanges))
            {
                string message = $"{SettingsVM.Locale.IncorrectPagesMessage}.";
                MainWindowMessage.ShowErrorDialog(message);
                return;
            }

            int[] pages = PageRange.GetAllItems(pageRanges);

            foreach (int page in pages)
            {
                RemovePage(page);
            }
        }

        private void TurnPageLeft(int pageNumber)
        {
            PageInfo? page = Pages.FirstOrDefault(t => t.PageNumber == pageNumber);      
            page?.TurnLeft90();
        }

        private void TurnPageRight(int pageNumber)
        {
            PageInfo? page = Pages.FirstOrDefault(t => t.PageNumber == pageNumber);      
            page?.TurnRight90();
        }

        private void RemovePage(int pageNumber)
        {
            PageInfo? page = Pages.FirstOrDefault(t => t.PageNumber == pageNumber);

            if (page is not null)
                Pages.Remove(page);
        }

        private void UpdatePagePositions()
        {
            for (int i = 0; i < Pages.Count; ++i)
            {
                Pages[i].Position = i + 1;
            }
        }

        private bool TryLoadPdfConfig(string path)
        {
            int pageCount;
            
            try
            {
                pageCount = PdfInfo.GetPageCount(path);
            }
            catch
            {
                return false;
            }

            if (pageCount == 0)
                return false;

            _sourcePdfFilePageCount = pageCount;

            Pages.Clear();
            
            for (int i = 1; i <= pageCount; ++i)
                Pages.Add(new PageInfo(i, i));

            return true;
        }

        private bool TryParsePageRanges(string data, out PageRange[] pageRanges)
        {
            try
            {
                pageRanges = AbstractParser.ParseMany(data, ',', PageRange.Parse);
                return true;
            }
            catch
            {
                pageRanges = Array.Empty<PageRange>();
                return false;
            }
        }

        #endregion
    }
}
