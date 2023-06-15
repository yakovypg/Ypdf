using Avalonia.Threading;
using ExecutionLib.Configuration;
using ReactiveUI;
using System.IO;
using System.Reactive;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Models;
using YpdfDesktop.Models.IO;
using YpdfDesktop.ViewModels.Base;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class ExtractImagesViewModel : PdfToolViewModel, IFilePathContainer
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectInputFilePathCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectOutputDirectoryPathCommand { get; }

        #endregion

        #region Reactive Properties

        private bool _isInputFilePathSelected = false;
        public bool IsInputFilePathSelected
        {
            get => _isInputFilePathSelected;
            private set => this.RaiseAndSetIfChanged(ref _isInputFilePathSelected, value);
        }

        private bool _isOutputDirectoryPathSelected = false;
        public bool IsOutputDirectoryPathSelected
        {
            get => _isOutputDirectoryPathSelected;
            private set => this.RaiseAndSetIfChanged(ref _isOutputDirectoryPathSelected, value);
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

        private string _outputDirectoryPath = string.Empty;
        public string OutputDirectoryPath
        {
            get => _outputDirectoryPath;
            private set
            {
                this.RaiseAndSetIfChanged(ref _outputDirectoryPath, value);
                IsOutputDirectoryPathSelected = !string.IsNullOrEmpty(value);
            }
        }

        #endregion

        // Constructor for Designer
        public ExtractImagesViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public ExtractImagesViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            ExecuteCommand = ReactiveCommand.Create(Execute);
            ResetCommand = ReactiveCommand.Create(Reset);
            SelectInputFilePathCommand = ReactiveCommand.Create(SelectInputFilePath);
            SelectOutputDirectoryPathCommand = ReactiveCommand.Create(SelectOutputDirectoryPath);
        }

        #region Protected Methods

        protected override void Execute()
        {
            var config = new YpdfConfig()
            {
                PdfTool = "extract-images"
            };

            config.PathsConfig.InputPath = InputFilePath;
            config.PathsConfig.OutputDirectory = OutputDirectoryPath;

            Execute(ToolType.ExtractImages, config, false);
        }

        protected override void Reset()
        {
            InputFilePath = string.Empty;
            OutputDirectoryPath = string.Empty;
        }

        #endregion

        #region Private Methods

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

        private void SelectOutputDirectoryPath()
        {
            _ = DialogProvider.GetDirectoryPath().ContinueWith(t =>
            {
                if (t.Result is not null && !string.IsNullOrEmpty(t.Result) && Directory.Exists(t.Result))
                    OutputDirectoryPath = t.Result;
            });
        }

        #endregion
    }
}