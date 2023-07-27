using Avalonia.Threading;
using ExecutionLib.Configuration;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Models;
using YpdfDesktop.Models.IO;
using YpdfDesktop.ViewModels.Base;
using YpdfLib.Models.Enumeration;
using YpdfLib.Models.Parsing;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class RemoveWatermarkViewModel : PdfToolViewModel, IFilePathContainer
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectInputFilePathCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectOutputFilePathCommand { get; }

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

        private string _pagesToRemoveWatermark = string.Empty;
        public string PagesToRemoveWatermark
        {
            get => _pagesToRemoveWatermark;
            set => this.RaiseAndSetIfChanged(ref _pagesToRemoveWatermark, value);
        }

        #endregion

        // Constructor for Designer
        public RemoveWatermarkViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public RemoveWatermarkViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            ExecuteCommand = ReactiveCommand.Create(Execute);
            ResetCommand = ReactiveCommand.Create(Reset);
            SelectInputFilePathCommand = ReactiveCommand.Create(SelectInputFilePath);
            SelectOutputFilePathCommand = ReactiveCommand.Create(SelectOutputFilePath);
        }

        #region Protected Methods

        protected override void Execute()
        {
            if (!VerifyOutputFilePath())
                return;
            
            var config = new YpdfConfig()
            {
                PdfTool = "remove-watermark-annotation"
            };

            config.PathsConfig.InputPath = InputFilePath;
            config.PathsConfig.OutputPath = CorrectOutputFilePath(OutputFilePath, "pdf");

            PageRange[] pages = ParsePagesToRemoveWatermark();
            Array.ForEach(pages, config.Pages.Add);

            Execute(ToolType.RemoveWatermark, config, true);
        }

        protected override void Reset()
        {
            InputFilePath = string.Empty;
            OutputFilePath = string.Empty;
            PagesToRemoveWatermark = string.Empty;
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

        private void SelectOutputFilePath()
        {
            const string initialFileName = "PdfWithoutWatermark";

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

        private PageRange[] ParsePagesToRemoveWatermark()
        {
            if (string.IsNullOrEmpty(PagesToRemoveWatermark))
                return Array.Empty<PageRange>();

            try
            {
                return AbstractParser.ParseMany(PagesToRemoveWatermark, ',', PageRange.Parse);
            }
            catch
            {
                string message = $"{SettingsVM.Locale.IncorrectPagesMessage}.";
                MainWindowMessage.ShowErrorDialog(message);

                return Array.Empty<PageRange>();
            }
        }

        #endregion
    }
}
