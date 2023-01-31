using Avalonia.Threading;
using ExecutionLib.Configuration;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Models;
using YpdfDesktop.Models.Enumeration;
using YpdfDesktop.ViewModels.Base;
using YpdfLib.Informing;
using YpdfLib.Models.Enumeration;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class SplitViewModel : PdfToolViewModel
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectFileCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectOutputDirectoryCommand { get; }
        public ReactiveCommand<Unit, Unit> AddPageRangeCommand { get; }
        public ReactiveCommand<IRange, Unit> DeletePageRangeCommand { get; }

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

        private string _outputDirectoryPath = string.Empty;
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
        public SplitViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public SplitViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
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
            _ = GetPdfFilePath().ContinueWith(t =>
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
                        throw new FileLoadException(SettingsVM.Locale.FileEmptyMessage, path);

                    PageRanges.Clear();
                    PageRanges.Add(new Models.Enumeration.Range(1, filePages));

                    if (string.IsNullOrEmpty(OutputDirectoryPath))
                        OutputDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    _filePages = filePages;
                    FilePath = path;
                    IsFileSelected = true;
                }
                catch (Exception ex)
                {
                    Dispatcher.UIThread.Post(() => MainWindowMessage.ShowErrorDialog(ex.Message));
                }
            });
        }

        private void SelectOutputDirectory()
        {
            _ = GetDirectoryPath().ContinueWith(t =>
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

            Execute(ToolType.Split, config, true);
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
