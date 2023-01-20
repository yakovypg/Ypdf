using Avalonia.Media;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Infrastructure.Default;
using YpdfDesktop.Models;
using YpdfDesktop.Models.Localization;
using YpdfDesktop.Models.Themes;

namespace YpdfDesktop.ViewModels
{
    public class ToolsViewModel : ViewModelBase
    {
        #region Commands

        public ReactiveCommand<ToolType, Unit> ChangeToolAffiliationToFavoritesCommand { get; }
        public ReactiveCommand<ToolType, Unit> ShowToolPageCommand { get; }

        #endregion

        #region ViewModels

        public SettingsViewModel SettingsVM { get; }

        #endregion

        #region Observable Collections

        public ObservableCollection<Tool> Tools { get; }
        public ObservableCollection<Tool> FavoriteTools { get; }

        #endregion

        #region Private Fields

        // Flag to prevent calling the ShowToolPage method
        private bool _isChangeToolAffiliationToFavoritesMethodInvoked = false;

        #endregion

        // Constructor for Designer
        public ToolsViewModel() : this(new SettingsViewModel(), null, null)
        {
        }

        public ToolsViewModel(SettingsViewModel settingsVM) : this(settingsVM, null, null)
        {
        }

        public ToolsViewModel(SettingsViewModel settingsVM, ObservableCollection<Tool>? tools, ObservableCollection<Tool>? favoriteTools = null)
        {
            SettingsVM = settingsVM;

            Tools = tools ?? DefaultTools.Get(settingsVM.Locale, settingsVM.Theme);
            FavoriteTools = favoriteTools ?? new ObservableCollection<Tool>();

            ChangeToolAffiliationToFavoritesCommand = ReactiveCommand.Create<ToolType>(ChangeToolAffiliationToFavorites);
            ShowToolPageCommand = ReactiveCommand.Create<ToolType>(ShowToolPage);
        }

        #region Public Methods

        public void UpdateLocale(ILocale locale)
        {
            if (locale is null)
                return;

            foreach (Tool tool in Tools)
            {
                tool.Name = tool.Type switch
                {
                    ToolType.Split => locale.Split ?? string.Empty,
                    ToolType.Merge => locale.Merge ?? string.Empty,
                    ToolType.Compress => locale.Compress ?? string.Empty,
                    ToolType.HandlePages => locale.HandlePages ?? string.Empty,
                    ToolType.CropPages => locale.CropPages ?? string.Empty,
                    ToolType.DividePages => locale.DividePages ?? string.Empty,
                    ToolType.AddPageNumbers => locale.AddPageNumbers ?? string.Empty,
                    ToolType.AddWatermark => locale.AddWatermark ?? string.Empty,
                    ToolType.RemoveWatermark => locale.RemoveWatermark ?? string.Empty,
                    ToolType.ImageToPdf => locale.ImageToPdf ?? string.Empty,
                    ToolType.TextToPdf => locale.TextToPdf ?? string.Empty,
                    ToolType.ExtractImages => locale.ExtractImages ?? string.Empty,
                    ToolType.ExtractText => locale.ExtractText ?? string.Empty,
                    ToolType.SetPassword => locale.SetPassword ?? string.Empty,
                    ToolType.RemovePassword => locale.RemovePassword ?? string.Empty,         
                    _ => string.Empty,
                };
            }
        }

        public void UpdateTheme(IWindowTheme theme)
        {
            if (theme is null)
                return;

            foreach (Tool tool in Tools)
            {
                tool.FavoriteStarBrush = theme.FavoriteStarIconForeground ?? Brushes.Black;
                tool.NotFavoriteStarBrush = theme.NotFavoriteStarIconForeground ?? Brushes.Black;
            }
        }

        #endregion

        #region Private Methods

        private void ChangeToolAffiliationToFavorites(ToolType toolType)
        {
            _isChangeToolAffiliationToFavoritesMethodInvoked = true;

            Tool tool = Tools.First(t => t.Type == toolType);

            if (tool.IsFavorite)
                FavoriteTools.Remove(tool);
            else
                FavoriteTools.Add(tool);
            
            tool.IsFavorite = !tool.IsFavorite;
        }

        private void ShowToolPage(ToolType toolType)
        {
            if (_isChangeToolAffiliationToFavoritesMethodInvoked)
            {
                _isChangeToolAffiliationToFavoritesMethodInvoked = false;
                return;
            }

            Tool tool = Tools.First(t => t.Type == toolType);
        }

        #endregion
    }
}
