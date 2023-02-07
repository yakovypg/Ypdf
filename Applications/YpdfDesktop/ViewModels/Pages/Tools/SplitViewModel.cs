using Avalonia.Input;
using Avalonia.Interactivity;
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
        public ReactiveCommand<KeyEventArgs, Unit> PreventNotDigitsInputCommand { get; }
        public ReactiveCommand<RoutedEventArgs, Unit> PreventTextInsertionCommand { get; }

        #endregion

        #region Reactive Properties

        private bool _isInputFileSelected = false;
        public bool IsInputFileSelected
        {
            get => _isInputFileSelected;
            private set => this.RaiseAndSetIfChanged(ref _isInputFileSelected, value);
        }

        private string _inputFilePath = string.Empty;
        public string InputFilePath
        {
            get => _inputFilePath;
            private set
            {
                this.RaiseAndSetIfChanged(ref _inputFilePath, value);
                IsInputFileSelected = !string.IsNullOrEmpty(value);
            }
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
            PreventNotDigitsInputCommand = ReactiveCommand.Create<KeyEventArgs>(PreventNotDigitsInput);
            PreventTextInsertionCommand = ReactiveCommand.Create<RoutedEventArgs>(PreventTextInsertion);
        }

        #region Public Methods

        public bool SetFilePath(string path)
        {
            if (!File.Exists(path))
                return false;

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
                InputFilePath = path;

                return true;
            }
            catch (Exception ex)
            {
                MainWindowMessage.ShowErrorDialog(ex.Message);
                return false;
            }
        }

        #endregion

        #region Protected Methods

        protected override void Execute()
        {
            var config = new YpdfConfig()
            {
                PdfTool = "split"
            };

            config.PathsConfig.InputPath = InputFilePath;
            config.PathsConfig.OutputDirectory = OutputDirectoryPath;

            foreach (var range in PageRanges)
            {
                if (range.IsCorrect)
                    config.Pages.Add(new PageRange(range.Start, range.End));
            }

            Execute(ToolType.Split, config, true);
        }

        protected override void Reset()
        {
            PageRanges.Clear();

            InputFilePath = string.Empty;
            OutputDirectoryPath = string.Empty;

            _filePages = 0;
        }

        #endregion

        #region Private Methods

        private void SelectFile()
        {
            _ = DialogProvider.GetPdfFilePaths().ContinueWith(t =>
            {
                if (t.Result is not null && t.Result.Length > 0)
                    SetFilePath(t.Result[0]);
            });
        }

        private void SelectOutputDirectory()
        {
            _ = DialogProvider.GetDirectoryPath().ContinueWith(t =>
            {
                if (t.Result is not null && !string.IsNullOrEmpty(t.Result) && Directory.Exists(t.Result))
                    OutputDirectoryPath = t.Result;
            });
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

        private void PreventNotDigitsInput(KeyEventArgs e)
        {
            if (e.KeyModifiers != KeyModifiers.None)
                e.Handled = true;

            if ((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                return;
            }

            e.Handled = true;
        }

        private void PreventTextInsertion(RoutedEventArgs e)
        {
            e.Handled = true;
        }

        #endregion
    }
}
