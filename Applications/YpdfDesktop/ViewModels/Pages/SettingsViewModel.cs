using ExecutionLib.Configuration;
using ReactiveUI;
using RuntimeLib.Python;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Infrastructure.Default;
using YpdfDesktop.Infrastructure.Services;
using YpdfDesktop.Models.Informing;
using YpdfDesktop.Models.Localization;
using YpdfDesktop.Models.Themes;
using YpdfDesktop.ViewModels.Base;

namespace YpdfDesktop.ViewModels.Pages
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Events

        public delegate void LocaleUpdatedHandler(ILocale locale);
        public event LocaleUpdatedHandler? LocaleUpdated;

        public delegate void ThemeUpdatedHandler(IWindowTheme theme);
        public event ThemeUpdatedHandler? ThemeUpdated;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> SaveSettingsCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenVkPageCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenGitHubPageCommand { get; }

        #endregion

        #region Reactive Properties

        private ILocale _locale = DefaultLocales.English;
        public ILocale Locale
        {
            get => _locale;
            set
            {
                this.RaiseAndSetIfChanged(ref _locale, value);
                LocaleUpdated?.Invoke(value);
            }
        }

        private IWindowTheme _theme = DefaultThemes.Light;
        public IWindowTheme Theme
        {
            get => _theme;
            set
            {
                this.RaiseAndSetIfChanged(ref _theme, value);
                ThemeUpdated?.Invoke(value);
            }
        }

        public IAppInfo _appVersion = AppInfoService.GetAppInfo();
        public IAppInfo AppVersion
        {
            get => _appVersion;
            set => this.RaiseAndSetIfChanged(ref _appVersion, value);
        }

        private string _pythonAlias = string.Empty;
        public string PythonAlias
        {
            get => _pythonAlias;
            set => this.RaiseAndSetIfChanged(ref _pythonAlias, value);
        }

        #endregion

        #region Observable Collections

        public ObservableCollection<Locale> Locales { get; }
        public ObservableCollection<WindowTheme> Themes { get; }

        #endregion

        #region Private Fields

        private readonly YpdfGlobalConfig _ypdfGlobalConfig = new();

        #endregion

        #region Constants

        private const string VK_PAGE_URL = "";
        private const string GITHUB_PAGE_URL = "https://github.com/yakovypg/Ypdf";

        #endregion

        public SettingsViewModel()
        {
            UpdateGlobalConfigFields();

            Locales = LocalesService.TryLoadLocales(out ObservableCollection<Locale> locales) && locales.Count > 0
                ? locales
                : DefaultLocales.Get();

            Themes = ThemesService.TryLoadThemes(out ObservableCollection<WindowTheme> themes) && themes.Count > 0
                ? themes
                : DefaultThemes.Get();

            SaveSettingsCommand = ReactiveCommand.Create(TrySaveSettings);
            OpenVkPageCommand = ReactiveCommand.Create(OpenVkPage);
            OpenGitHubPageCommand = ReactiveCommand.Create(OpenGitHubPage);
        }

        #region Private Methods

        private void TrySaveSettings()
        {
            try
            {
                SaveGlobalConfigFields();
            }
            catch (Exception ex)
            {
                MainWindowMessage.ShowErrorDialog(ex.Message);
            }
        }

        private void OpenVkPage()
        {
            _ = UrlService.TryOpenUrl(VK_PAGE_URL);
        }

        private void OpenGitHubPage()
        {
            _ = UrlService.TryOpenUrl(GITHUB_PAGE_URL);
        }

        private void UpdateGlobalConfigFields()
        {
            if (_ypdfGlobalConfig.PythonAlias is null)
            {
                PythonDetector.DetectPythonAlias(out string alias);
                PythonAlias = alias;
            }
            else
            {
                PythonAlias = _ypdfGlobalConfig.PythonAlias;
            }
        }

        private void SaveGlobalConfigFields()
        {
            _ypdfGlobalConfig.PythonAlias = PythonAlias;
            _ypdfGlobalConfig.Save();
        }

        #endregion
    }
}
