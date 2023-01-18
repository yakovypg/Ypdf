using ReactiveUI;
using System.Diagnostics;
using System.Reactive;
using System.Reflection;

namespace YpdfDesktop.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> SaveSettingsCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenVkPageCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenGitHubPageCommand { get; }

        #endregion

        #region Properties

        private string _pythonAlias = string.Empty;
        public string PythonAlias
        {
            get => _pythonAlias;
            set => this.RaiseAndSetIfChanged(ref _pythonAlias, value);
        }

        public string _appVersion = GetAppVersion();
        public string AppVersion
        {
            get => _appVersion;
            set => this.RaiseAndSetIfChanged(ref _appVersion, value);
        }

        #endregion

        private const string VK_PAGE_URL = "";
        private const string GITHUB_PAGE_URL = "https://github.com/yakovypg/Ypdf";

        public SettingsViewModel()
        {
            SaveSettingsCommand = ReactiveCommand.Create(SaveSettings);
            OpenVkPageCommand = ReactiveCommand.Create(OpenVkPage);
            OpenGitHubPageCommand = ReactiveCommand.Create(OpenGitHubPage);
        }

        private void SaveSettings()
        {

        }

        private void OpenVkPage()
        {
            if (string.IsNullOrEmpty(VK_PAGE_URL))
                return;

            _ = TryOpenLink(VK_PAGE_URL);
        }

        private void OpenGitHubPage()
        {
            if (string.IsNullOrEmpty(GITHUB_PAGE_URL))
                return;

            _ = TryOpenLink(GITHUB_PAGE_URL);
        }

        private static string GetAppVersion()
        {
            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
            string? name = assemblyName.Name?.ToString();
            string? version = assemblyName.Version?.ToString();

            return $"{name} {version}";
        }

        private static bool TryOpenLink(string url)
        {
            try
            {
                var startInfo = new ProcessStartInfo(url) { UseShellExecute = true };
                return Process.Start(startInfo) is not null;
            }
            catch
            {
                return false;
            }
        }
    }
}
