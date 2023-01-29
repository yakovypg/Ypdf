using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using ExecutionLib.Configuration;
using ExecutionLib.Execution;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Infrastructure.Search;
using YpdfDesktop.Models.Enumeration;
using YpdfLib.Informing;
using YpdfLib.Models.Enumeration;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class SplitViewModel : ViewModelBase
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectFileCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectOutputDirectoryCommand { get; }
        public ReactiveCommand<Unit, Unit> AddPageRangeCommand { get; }
        public ReactiveCommand<IRange, Unit> DeletePageRangeCommand { get; }

        #endregion

        #region View Models

        public SettingsViewModel SettingsVM { get; }

        #endregion

        #region Reactive Properties

        private bool _isFileSelected = false;
        public bool IsFileSelected
        {
            get => _isFileSelected;
            private set => this.RaiseAndSetIfChanged(ref _isFileSelected, value);
        }

        private string _filePath = string.Empty;
        public string FilePath
        {
            get => _filePath;
            private set => this.RaiseAndSetIfChanged(ref _filePath, value);
        }

        private string _outputDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public string OutputDirectoryPath
        {
            get => _outputDirectoryPath;
            private set => this.RaiseAndSetIfChanged(ref _outputDirectoryPath, value);
        }

        #endregion

        #region Observable Collections

        public ObservableCollection<IRange> PageRanges { get; }

        #endregion

        #region Private Fields

        private int _filePages = 0;

        #endregion

        // Constructor for Designer
        public SplitViewModel() : this(new SettingsViewModel())
        {
        }

        public SplitViewModel(SettingsViewModel settingsVM)
        {
            SettingsVM = settingsVM;
            PageRanges = new ObservableCollection<IRange>();

            ExecuteCommand = ReactiveCommand.Create(Execute);
            ResetCommand = ReactiveCommand.Create(Reset);
            SelectFileCommand = ReactiveCommand.Create(SelectFile);
            SelectOutputDirectoryCommand = ReactiveCommand.Create(SelectOutputDirectory);
            AddPageRangeCommand = ReactiveCommand.Create(AddPageRange);
            DeletePageRangeCommand = ReactiveCommand.Create<IRange>(DeletePageRange);
        }

        #region Private Methods

        private void SelectFile()
        {
            var dialog = new OpenFileDialog()
            {
                AllowMultiple = false,
                Title = "Select PDF file",
                
                Filters = new List<FileDialogFilter>()
                {
                    new FileDialogFilter() { Name = "PDF Documents", Extensions = new List<string>() { "pdf" } }
                }
            };

            if (WindowFinder.FindMainWindow() is not Window mainWindow)
                return;

            dialog.ShowAsync(mainWindow).ContinueWith(t =>
            {
                if (t.Result is null || t.Result.Length == 0)
                    return;

                string path = t.Result[0];

                if (!File.Exists(path))
                    return;

                try
                {
                    int filePages = PdfInfo.GetPageCount(path);

                    if (filePages == 0)
                        throw new FileLoadException("File is empty.", path);

                    PageRanges.Clear();

                    _filePages = filePages;
                    FilePath = path;
                    IsFileSelected = true;
                }
                catch (Exception ex)
                {
                    Dispatcher.UIThread.Post(() => new QuickMessage(ex.Message).ShowError());
                    return;
                }
            });
        }

        private void SelectOutputDirectory()
        {
            var dialog = new OpenFolderDialog()
            {
                Title = "Select output directory path"
            };

            if (WindowFinder.FindMainWindow() is not Window mainWindow)
                return;

            dialog.ShowAsync(mainWindow).ContinueWith(t =>
            {
                if (t.Result is null || string.IsNullOrEmpty(t.Result) || !Directory.Exists(t.Result))
                    return;

                OutputDirectoryPath = t.Result;
            });
        }

        private void Execute()
        {
            var config = new YpdfConfig()
            {
                PdfTool = "split"
            };

            config.PathsConfig.InputPath = FilePath;
            config.PathsConfig.OutputDirectory = OutputDirectoryPath;

            foreach (var range in PageRanges)
            {
                if (range.IsCorrect)
                    config.Pages.Add(new PageRange(range.Start, range.End));
            }

            var executor = new PdfToolExecutor(config)
            {
                Logger = null,
                FileExistsQuestion = null,
                ApplyCorrectionsQuestion = null
            };

            IExecutionInfo executionInfo = executor.PrepareExecute();
            executor.Execute(executionInfo);
        }

        private void Reset()
        {
            PageRanges.Clear();

            FilePath = string.Empty;
            OutputDirectoryPath = string.Empty;
            IsFileSelected = false;

            _filePages = 0;
        }

        private void AddPageRange()
        {
            var range = new Models.Enumeration.Range(1, _filePages);
            PageRanges.Add(range);
        }

        private void DeletePageRange(IRange range)
        {
            PageRanges.Remove(range);
        }

        #endregion
    }
}
