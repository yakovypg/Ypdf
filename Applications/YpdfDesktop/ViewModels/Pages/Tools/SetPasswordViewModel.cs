using Avalonia.Threading;
using ExecutionLib.Configuration;
using ExecutionLib.Informing.Aliases;
using iText.Kernel.Pdf;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Models;
using YpdfDesktop.Models.IO;
using YpdfDesktop.ViewModels.Base;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class SetPasswordViewModel : PdfToolViewModel, IFilePathContainer
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

        private bool _isAnyPasswordSpecified = false;
        public bool IsAnyPasswordSpecified
        {
            get => _isAnyPasswordSpecified;
            private set => this.RaiseAndSetIfChanged(ref _isAnyPasswordSpecified, value);
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

        private string _ownerPassword = string.Empty;
        public string OwnerPassword
        {
            get => _ownerPassword;
            set
            {
                this.RaiseAndSetIfChanged(ref _ownerPassword, value);
                IsAnyPasswordSpecified = !string.IsNullOrEmpty(OwnerPassword) || !string.IsNullOrEmpty(UserPassword);
            }
        }

        private string _userPassword = string.Empty;
        public string UserPassword
        {
            get => _userPassword;
            set
            {
                this.RaiseAndSetIfChanged(ref _userPassword, value);
                IsAnyPasswordSpecified = !string.IsNullOrEmpty(OwnerPassword) || !string.IsNullOrEmpty(UserPassword);
            }
        }

        private string _encryptionAlgorithm = string.Empty;
        public string EncryptionAlgorithm
        {
            get => _encryptionAlgorithm;
            set => this.RaiseAndSetIfChanged(ref _encryptionAlgorithm, value);
        }

        private bool _showPassword = false;
        public bool ShowPassword
        {
            get => _showPassword;
            set
            {
                this.RaiseAndSetIfChanged(ref _showPassword, value);
                PasswordChar = value ? null : DEFAULT_PASSWORD_CHAR;
            }
        }

        private char? _passwordChar = DEFAULT_PASSWORD_CHAR;
        public char? PasswordChar
        {
            get => _passwordChar;
            private set => this.RaiseAndSetIfChanged(ref _passwordChar, value);
        }

        #endregion

        #region Observable Collections

        public ObservableCollection<string> EncryptionAlgorithms { get; }

        #endregion

        #region Constants

        private const char DEFAULT_PASSWORD_CHAR = '*';

        #endregion

        // Constructor for Designer
        public SetPasswordViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public SetPasswordViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            var encryptionAlgorithms = StandardValues.EncryptionAlgorithms.Keys;
            EncryptionAlgorithms = new ObservableCollection<string>(encryptionAlgorithms);

            ExecuteCommand = ReactiveCommand.Create(Execute);
            ResetCommand = ReactiveCommand.Create(Reset);
            SelectInputFilePathCommand = ReactiveCommand.Create(SelectInputFilePath);
            SelectOutputFilePathCommand = ReactiveCommand.Create(SelectOutputFilePath);

            SetDefaultItems();
        }

        #region Protected Methods

        protected override void Execute()
        {
            if (!VerifyOutputFilePath() || !VerifyPasswords())
                return;

            var config = new YpdfConfig()
            {
                PdfTool = "set-password"
            };

            config.PathsConfig.InputPath = InputFilePath;
            config.PathsConfig.OutputPath = CorrectOutputFilePath(OutputFilePath, "pdf");

            if (!string.IsNullOrEmpty(OwnerPassword))
                config.PdfPassword.OwnerPassword = OwnerPassword;

            if (!string.IsNullOrEmpty(UserPassword))
                config.PdfPassword.UserPassword = UserPassword;

            int encryptionAlgorithm = StandardValues.EncryptionAlgorithms[EncryptionAlgorithm];
            config.PdfPassword.EncryptionAlgorithm = encryptionAlgorithm;

            Execute(ToolType.SetPassword, config, true);
        }

        protected override void Reset()
        {
            InputFilePath = string.Empty;
            OutputFilePath = string.Empty;
            OwnerPassword = string.Empty;
            UserPassword = string.Empty;

            SetDefaultEncryptionAlgorithm();
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
            const string initialFileName = "Encrypted";

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

        private bool VerifyPasswords()
        {
            return InformIfIncorrect(IsAnyPasswordSpecified, SettingsVM.Locale.SpecifyAtLeastOnePasswordMessage);
        }

        private void SetDefaultItems()
        {
            SetDefaultEncryptionAlgorithm();
        }

        private void SetDefaultEncryptionAlgorithm()
        {
            var aes256 = StandardValues.EncryptionAlgorithms
                .First(t => t.Value == EncryptionConstants.ENCRYPTION_AES_256);

            if (EncryptionAlgorithms.Contains(aes256.Key))
                EncryptionAlgorithm = aes256.Key;
            else if (EncryptionAlgorithms.Count > 0)
                EncryptionAlgorithm = EncryptionAlgorithms[0];
            else
                EncryptionAlgorithm = string.Empty;
        }

        #endregion
    }
}
