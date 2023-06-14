using Avalonia.Controls;
using Avalonia.Threading;
using MessageBox.Avalonia.Enums;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Infrastructure.Default;
using YpdfDesktop.Infrastructure.Search;
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

        public ReactiveCommand<CancelEventArgs, Unit> CheckRunningTasksCommand { get; }
        public ReactiveCommand<CancelEventArgs, Unit> SaveUIConfigurationCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowFavoriteToolsCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowToolsCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowSettingsCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowTasksCommand { get; }

        #endregion

        #region ViewModels

        public ToolsViewModel FavoriteToolsVM { get; }
        public ToolsViewModel ToolsVM { get; }
        public SettingsViewModel SettingsVM { get; }
        public TasksViewModel TasksVM { get; }

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

        private bool _isTasksVisible = false;
        public bool IsTasksVisible
        {
            get => _isTasksVisible;
            private set => this.RaiseAndSetIfChanged(ref _isTasksVisible, value);
        }

        #endregion Properties

        #region Private Fields

        private bool _isRunningTasksChecked = false;

        #endregion

        public MainWindowViewModel()
        {
            SharedConfig.Directories.Prepare();
            SharedConfig.Files.Prepare();

            SettingsVM = new SettingsViewModel();
            TasksVM = new TasksViewModel(SettingsVM);

            var allTools = DefaultTools.Get(SettingsVM.Locale, SettingsVM.Theme);
            var favoritesTools = new ObservableCollection<Tool>();

            FavoriteToolsVM = new ToolsViewModel(SettingsVM, TasksVM, favoritesTools, favoritesTools);
            ToolsVM = new ToolsViewModel(SettingsVM, TasksVM, allTools, favoritesTools);

            SettingsVM.LocaleUpdated += FavoriteToolsVM.UpdateLocale;
            SettingsVM.LocaleUpdated += ToolsVM.UpdateLocale;
            SettingsVM.ThemeUpdated += FavoriteToolsVM.UpdateTheme;
            SettingsVM.ThemeUpdated += ToolsVM.UpdateTheme;

            CheckRunningTasksCommand = ReactiveCommand.Create<CancelEventArgs>(CheckRunningTasks);
            SaveUIConfigurationCommand = ReactiveCommand.Create<CancelEventArgs>(SaveUIConfiguration);
            ShowFavoriteToolsCommand = ReactiveCommand.Create(ShowFavoriteTools);
            ShowToolsCommand = ReactiveCommand.Create(ShowTools);
            ShowSettingsCommand = ReactiveCommand.Create(ShowSettings);
            ShowTasksCommand = ReactiveCommand.Create(ShowTasks);

            LoadUIConfiguration();
        }

        #region Private Methods

        private void LoadUIConfiguration()
        {
            if (!UIConfigService.TryLoadUIConfiguration(out UIConfig config))
                return;

            Locale? selectedLocale = config.SelectedLocaleId is not null
                ? SettingsVM.Locales.FirstOrDefault(t => t.Id == config.SelectedLocaleId)
                : SettingsVM.Locales.FirstOrDefault(t => t.Id == DefaultLocales.Get()[0].Id);

            if (selectedLocale is not null)
                SettingsVM.Locale = selectedLocale;
            else if (SettingsVM.Locales.Count > 0)
                SettingsVM.Locale = SettingsVM.Locales[0];

            WindowTheme? selectedTheme = config.SelectedThemeId is not null
                ? SettingsVM.Themes.FirstOrDefault(t => t.Id == config.SelectedThemeId)
                : SettingsVM.Themes.FirstOrDefault(t => t.Id == DefaultThemes.Get()[0].Id);

            if (selectedTheme is not null)
                SettingsVM.Theme = selectedTheme;
            else if (SettingsVM.Themes.Count > 0)
                SettingsVM.Theme = SettingsVM.Themes[0];

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

        private void CheckRunningTasks(CancelEventArgs e)
        {
            if (_isRunningTasksChecked || TasksVM.RunningTasksCount == 0)
                return;

            if (WindowFinder.FindMainWindow() is not Window window)
                return;

            string? unfinishedTasksMsg = SettingsVM.Locale.UnfinishedTasksMessage;
            string? exitWithoutWaitingForCompletionMsg = SettingsVM.Locale.ExitWithoutWaitingForCompletionMessage;

            QuickMessage quickMessage = new($"{unfinishedTasksMsg}. {exitWithoutWaitingForCompletionMsg}?");

            _ = quickMessage.ShowQuestionDialog(window).ContinueWith(t =>
            {
                if (t.Result == ButtonResult.Yes)
                {
                    _isRunningTasksChecked = true;
                    Dispatcher.UIThread.Post(window.Close);
                }
            });

            e.Cancel = true;
        }

        private void SaveUIConfiguration(CancelEventArgs e)
        {
            if (e.Cancel)
                return;

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

        private void ShowTasks()
        {
            HideAllPages();
            IsTasksVisible = true;
        }

        private void HideAllToolPages()
        {
            FavoriteToolsVM.HideAllToolsPages();
            ToolsVM.HideAllToolsPages();
        }

        private void HideAllPages()
        {
            HideAllToolPages();

            IsFavoriteToolsVisible = false;
            IsToolsVisible = false;
            IsSettingsVisible = false;
            IsTasksVisible = false;
        }

        #endregion
    }
}
