using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using YpdfDesktop.Infrastructure.Support;
using YpdfDesktop.Models;

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

        private readonly ToolsViewModel _favoritesToolsVM;
        public ToolsViewModel FavoritesToolsVM => _favoritesToolsVM;

        private readonly ToolsViewModel _toolsVM;
        public ToolsViewModel ToolsVM => _toolsVM;

        private readonly SettingsViewModel _settingsVM;
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

        private readonly ObservableCollection<Tool> _allTools;
        private readonly ObservableCollection<Tool> _favoritesTools;

        public MainWindowViewModel()
        {
            _allTools = SupportedTools.Get();
            _favoritesTools = new ObservableCollection<Tool>();
            
            _favoritesToolsVM = new ToolsViewModel(_favoritesTools, _favoritesTools);
            _toolsVM = new ToolsViewModel(_allTools, _favoritesTools);
            _settingsVM = new SettingsViewModel();

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
