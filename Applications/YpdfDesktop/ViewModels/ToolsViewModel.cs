using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Infrastructure.Support;
using YpdfDesktop.Models;

namespace YpdfDesktop.ViewModels
{
    public class ToolsViewModel : ViewModelBase
    {
        #region Commands

        public ReactiveCommand<string, Unit> ChangeToolAffiliationToFavoritesCommand { get; }
        public ReactiveCommand<string, Unit> ShowToolPageCommand { get; }

        #endregion

        public ObservableCollection<Tool> Tools { get; }
        public ObservableCollection<Tool> FavoriteTools { get; }

        // Flag to prevent calling the ShowToolPage method
        private bool _isChangeToolAffiliationToFavoritesMethodInvoked = false;

        public ToolsViewModel() : this(null, null)
        {
        }

        public ToolsViewModel(ObservableCollection<Tool>? tools, ObservableCollection<Tool>? favoriteTools = null)
        {
            Tools = tools ?? SupportedTools.Get();
            FavoriteTools = favoriteTools ?? new ObservableCollection<Tool>();

            ChangeToolAffiliationToFavoritesCommand = ReactiveCommand.Create<string>(ChangeToolAffiliationToFavorites);
            ShowToolPageCommand = ReactiveCommand.Create<string>(ShowToolPage);
        }

        private void ChangeToolAffiliationToFavorites(string toolName)
        {
            _isChangeToolAffiliationToFavoritesMethodInvoked = true;

            Tool tool = Tools.First(t => t.Name == toolName);

            if (tool.IsFavorite)
                FavoriteTools.Remove(tool);
            else
                FavoriteTools.Add(tool);
            
            tool.IsFavorite = !tool.IsFavorite;
        }

        private void ShowToolPage(string toolName)
        {
            if (_isChangeToolAffiliationToFavoritesMethodInvoked)
            {
                _isChangeToolAffiliationToFavoritesMethodInvoked = false;
                return;
            }

            Tool tool = Tools.First(t => t.Name == toolName);
        }
    }
}
