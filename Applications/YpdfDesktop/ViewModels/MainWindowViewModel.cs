using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Infrastructure.Default;
using YpdfDesktop.Infrastructure.Services;
using YpdfDesktop.Models;
using YpdfDesktop.Models.Configuration;
using YpdfDesktop.Models.Localization;
using YpdfDesktop.Models.Themes;
using YpdfDesktop.ViewModels.Base;
using YpdfDesktop.ViewModels.Pages;

namespace YpdfDesktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> SaveUIConfigurationCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowFavoriteToolsCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowToolsCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowSettingsCommand { get; }

        #endregion

        #region ViewModels

        public ToolsViewModel FavoriteToolsVM { get; }
        public ToolsViewModel ToolsVM { get; }
        public SettingsViewModel SettingsVM { get; }

        #endregion

        #region Reactive Properties

        private bool _isFavoriteToolsVisible = true;
        public bool IsFavoriteToolsVisible
        {
            get => _isFavoriteToolsVisible;
            private set => this.RaiseAndSetIfChanged(ref _isFavoriteToolsVisible, value);
        }

        private bool _isToolsVisible = false;
        public bool IsToolsVisible
        {
            get => _isToolsVisible;
            private set => this.RaiseAndSetIfChanged(ref _isToolsVisible, value);
        }

        private bool _isSettingsVisible = false;
        public bool IsSettingsVisible
        {
            get => _isSettingsVisible;
            private set => this.RaiseAndSetIfChanged(ref _isSettingsVisible, value);
        }

        #endregion Properties

        public MainWindowViewModel()
        {
            SharedConfig.Directories.Prepare();
            SharedConfig.Files.Prepare();

            SettingsVM = new SettingsViewModel();

            var allTools = DefaultTools.Get(SettingsVM.Locale, SettingsVM.Theme);
            var favoritesTools = new ObservableCollection<Tool>();

            FavoriteToolsVM = new ToolsViewModel(SettingsVM, favoritesTools, favoritesTools);
            ToolsVM = new ToolsViewModel(SettingsVM, allTools, favoritesTools);

            SettingsVM.LocaleUpdated += FavoriteToolsVM.UpdateLocale;
            SettingsVM.LocaleUpdated += ToolsVM.UpdateLocale;
            SettingsVM.ThemeUpdated += FavoriteToolsVM.UpdateTheme;
            SettingsVM.ThemeUpdated += ToolsVM.UpdateTheme;

            SaveUIConfigurationCommand = ReactiveCommand.Create(SaveUIConfiguration);
            ShowFavoriteToolsCommand = ReactiveCommand.Create(ShowFavoriteTools);
            ShowToolsCommand = ReactiveCommand.Create(ShowTools);
            ShowSettingsCommand = ReactiveCommand.Create(ShowSettings);

            LoadUIConfiguration();
        }

        #region Private Methods

        private void LoadUIConfiguration()
        {
            if (!UIConfigService.TryLoadUIConfiguration(out UIConfig config))
                return;

            if (config.SelectedLocaleId is not null)
            {
                Locale? selectedLocale = SettingsVM.Locales.FirstOrDefault(t => t.Id == config.SelectedLocaleId);

                if (selectedLocale is not null)
                    SettingsVM.Locale = selectedLocale;
                else if (SettingsVM.Locales.Count > 0)
                    SettingsVM.Locale = SettingsVM.Locales[0];

            }

            if (config.SelectedThemeId is not null)
            {
                WindowTheme? selectedTheme = SettingsVM.Themes.FirstOrDefault(t => t.Id == config.SelectedThemeId);

                if (selectedTheme is not null)
                    SettingsVM.Theme = selectedTheme;
                else if (SettingsVM.Themes.Count > 0)
                    SettingsVM.Theme = SettingsVM.Themes[0];
            }

            if (config.ResetAfterExecution is not null)
            {
                SettingsVM.ResetAfterExecution = config.ResetAfterExecution.Value;
            }

            if (config.FavoriteTools is not null)
            {
                foreach (ToolType toolType in config.FavoriteTools)
                {
                    Tool? tool = ToolsVM.Tools.FirstOrDefault(t => t.Type == toolType);

                    if (tool is not null)
                    {
                        tool.IsFavorite = true;
                        FavoriteToolsVM.FavoriteTools.Add(tool);
                    }
                }
            }
        }

        private void SaveUIConfiguration()
        {
            List<ToolType> favoriteTools = FavoriteToolsVM.FavoriteTools
                .Select(t => t.Type)
                .ToList();

            var config = new UIConfig()
            {
                SelectedLocaleId = SettingsVM.Locale.Id,
                SelectedThemeId = SettingsVM.Theme.Id,
                ResetAfterExecution = SettingsVM.ResetAfterExecution,
                FavoriteTools = favoriteTools
            };

            _ = UIConfigService.TrySaveUIConfiguration(config);
        }

        private void ShowFavoriteTools()
        {
            HideAllPages();
            IsFavoriteToolsVisible = true;
        }

        private void ShowTools()
        {
            HideAllPages();
            IsToolsVisible = true;
        }

        private void ShowSettings()
        {
            HideAllPages();
            IsSettingsVisible = true;
        }

        private void HideAllPages()
        {
            IsFavoriteToolsVisible = false;
            IsToolsVisible = false;
            IsSettingsVisible = false;
        }

        #endregion
    }
}
