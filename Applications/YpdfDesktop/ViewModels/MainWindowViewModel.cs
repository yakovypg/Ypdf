using ReactiveUI;
using System.Reactive;

namespace YpdfDesktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> ShowFavoriteToolsCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowToolsCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowSettingsCommand { get; }

        #endregion

        #region ViewModels

        private readonly SettingsViewModel _favoritesToolsVM = new();
        public SettingsViewModel FavoritesToolsVM => _favoritesToolsVM;

        private readonly SettingsViewModel _toolsVN = new();
        public SettingsViewModel ToolsVN => _toolsVN;

        private readonly SettingsViewModel _settingsVM = new();
        public SettingsViewModel SettingsVM => _settingsVM;

        #endregion

        #region Properties

        private bool _isFavoriteToolsVisible = true;
        public bool IsFavoriteToolsVisible
        {
            get => _isFavoriteToolsVisible;
            set => this.RaiseAndSetIfChanged(ref _isFavoriteToolsVisible, value);
        }

        private bool _isToolsVisible = false;
        public bool IsToolsVisible
        {
            get => _isToolsVisible;
            set => this.RaiseAndSetIfChanged(ref _isToolsVisible, value);
        }

        private bool _isSettingsVisible = false;
        public bool IsSettingsVisible
        {
            get => _isSettingsVisible;
            set => this.RaiseAndSetIfChanged(ref _isSettingsVisible, value);
        }

        #endregion Properties

        public MainWindowViewModel()
        {
            ShowFavoriteToolsCommand = ReactiveCommand.Create(ShowFavoriteTools);
            ShowToolsCommand = ReactiveCommand.Create(ShowTools);
            ShowSettingsCommand = ReactiveCommand.Create(ShowSettings);
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
    }
}
