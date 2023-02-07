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
    public class CompressViewModel : PdfToolViewModel, IFilePathContainer
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

        private bool _isQualityFactorCorrect = true;
        public bool IsQualityFactorCorrect
        {
            get => _isQualityFactorCorrect;
            private set => this.RaiseAndSetIfChanged(ref _isQualityFactorCorrect, value);
        }

        private bool _isSizeFactorCorrect = true;
        public bool IsSizeFactorCorrect
        {
            get => _isSizeFactorCorrect;
            private set => this.RaiseAndSetIfChanged(ref _isSizeFactorCorrect, value);
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

        private float _qualityFactor = DEFAULT_QUALITY_FACTOR;
        public float QualityFactor
        {
            get => _qualityFactor;
            set
            {
                this.RaiseAndSetIfChanged(ref _qualityFactor, value);
                IsQualityFactorCorrect = value > 0 && value <= 1;
            }
        }

        private float _sizeFactor = DEFAULT_SIZE_FACTOR;
        public float SizeFactor
        {
            get => _sizeFactor;
            set
            {
                this.RaiseAndSetIfChanged(ref _sizeFactor, value);
                IsSizeFactorCorrect = _sizeFactor > 0;
            }
        }

        private string _extension = DEFAULT_EXTENSION;
        public string Extension
        {
            get => _extension;
            set => this.RaiseAndSetIfChanged(ref _extension, value);
        }

        private bool _checkCompressionValidity = DEFAULT_CHECK_COMPRESSION_VALIDITY_VALUE;
        public bool CheckCompressionValidity
        {
            get => _checkCompressionValidity;
            set => this.RaiseAndSetIfChanged(ref _checkCompressionValidity, value);
        }

        #endregion

        #region Constants

        private const float DEFAULT_QUALITY_FACTOR = 0.75f;
        private const float DEFAULT_SIZE_FACTOR = 1.0f;
        private const string DEFAULT_EXTENSION = "jpg";
        private const bool DEFAULT_CHECK_COMPRESSION_VALIDITY_VALUE = true;

        #endregion

        // Constructor for Designer
        public CompressViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public CompressViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            ExecuteCommand = ReactiveCommand.Create(Execute);
            ResetCommand = ReactiveCommand.Create(Reset);
            SelectInputFilePathCommand = ReactiveCommand.Create(SelectInputFilePath);
            SelectOutputFilePathCommand = ReactiveCommand.Create(SelectOutputFilePath);
        }

        #region Protected Methods

        protected override void Execute()
        {
            if (!VerifyOutputFilePath() || !VerifyQualityFactor() || !VerifySizeFactor())
                return;

            var config = new YpdfConfig()
            {
                PdfTool = "compress"
            };

            config.PathsConfig.InputPath = InputFilePath;
            config.PathsConfig.OutputPath = CorrectOutputFilePath(OutputFilePath, "pdf");

            config.ImageCompression.QualityFactor = QualityFactor;
            config.ImageCompression.SizeFactor = SizeFactor;
            config.ImageCompression.Extension = Extension;
            config.ImageCompression.CheckCompressionValidity = CheckCompressionValidity;

            Execute(ToolType.Compress, config, true);
        }

        protected override void Reset()
        {
            InputFilePath = string.Empty;
            OutputFilePath = string.Empty;
            QualityFactor = DEFAULT_QUALITY_FACTOR;
            SizeFactor = DEFAULT_SIZE_FACTOR;
            Extension = DEFAULT_EXTENSION;
            CheckCompressionValidity = DEFAULT_CHECK_COMPRESSION_VALIDITY_VALUE;
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
            const string initialFileName = "Compressed";

            _ = DialogProvider.GetOutputFilePath(initialFileName, true).ContinueWith(t =>
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

        private bool VerifyQualityFactor()
        {
            return InformIfIncorrect(IsQualityFactorCorrect, SettingsVM.Locale.SpecifyCorrectQualityFactorMessage);
        }

        private bool VerifySizeFactor()
        {
            return InformIfIncorrect(IsSizeFactorCorrect, SettingsVM.Locale.SpecifyCorrectSizeFactorMessage);
        }

        #endregion
    }
}
