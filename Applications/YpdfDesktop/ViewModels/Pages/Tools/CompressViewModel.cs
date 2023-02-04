using ReactiveUI;
using YpdfDesktop.ViewModels.Base;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class CompressViewModel : PdfToolViewModel
    {
        #region Commands

        #endregion

        #region Reactive Properties

        private bool _isInputFileSelected = false;
        public bool IsInputFileSelected
        {
            get => _isInputFileSelected;
            private set => this.RaiseAndSetIfChanged(ref _isInputFileSelected, value);
        }

        private bool _isOutputFileSelected = false;
        public bool IsOutputFileSelected
        {
            get => _isOutputFileSelected;
            private set => this.RaiseAndSetIfChanged(ref _isOutputFileSelected, value);
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

        private string _outputFilePath = string.Empty;
        public string OutputFilePath
        {
            get => _outputFilePath;
            private set
            {
                this.RaiseAndSetIfChanged(ref _outputFilePath, value);
                IsOutputFileSelected = !string.IsNullOrEmpty(value);
            }
        }

        private float _qualityFactor = 0.75f;
        public float QualityFactor
        {
            get => _qualityFactor;
            set => this.RaiseAndSetIfChanged(ref _qualityFactor, value);
        }

        private float _sizeFactor = 1.0f;
        public float SizeFactor
        {
            get => _sizeFactor;
            set => this.RaiseAndSetIfChanged(ref _sizeFactor, value);
        }

        private string _extension = "jpg";
        public string Extension
        {
            get => _extension;
            set => this.RaiseAndSetIfChanged(ref _extension, value);
        }

        private bool _checkCompressionValidity = true;
        public bool CheckCompressionValidity
        {
            get => _checkCompressionValidity;
            set => this.RaiseAndSetIfChanged(ref _checkCompressionValidity, value);
        }

        #endregion

        // Constructor for Designer
        public CompressViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public CompressViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
        }

        #region Protected Methods

        protected override void Execute()
        {
            throw new System.NotImplementedException();
        }

        protected override void Reset()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
