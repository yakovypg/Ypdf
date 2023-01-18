using Avalonia.Input;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
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

        // Flag to prevent calling the ShowToolPage method
        private bool _isChangeToolAffiliationToFavoritesMethodInvoked = false;

        public ToolsViewModel()
        {
            Tools = new ObservableCollection<Tool>();
            AddTools();

            ChangeToolAffiliationToFavoritesCommand = ReactiveCommand.Create<string>(ChangeToolAffiliationToFavorites);
            ShowToolPageCommand = ReactiveCommand.Create<string>(ShowToolPage);
        }

        private void ChangeToolAffiliationToFavorites(string toolName)
        {
            _isChangeToolAffiliationToFavoritesMethodInvoked = true;
            
            Tool tool = Tools.First(t => t.Name == toolName);
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

        private void AddTools()
        {
            Tools.Add(new Tool("split", "fa-scissors"));
            Tools.Add(new Tool("merge", "fa-copy"));
            Tools.Add(new Tool("compress", "fa-compress"));
            Tools.Add(new Tool("remove pages", "fa-cog"));
            Tools.Add(new Tool("reorder pages", "fa-cog"));
            Tools.Add(new Tool("rotate pages", "fa-cog"));
            Tools.Add(new Tool("crop pages", "fa-crop"));
            Tools.Add(new Tool("divide pages", "fa-columns"));
            Tools.Add(new Tool("add page nums", "fa-sort-numeric-down"));
            Tools.Add(new Tool("add watermark", "fa-paint-brush"));
            Tools.Add(new Tool("remove watermark", "fa-eraser"));
            Tools.Add(new Tool("image2pdf", "fa-file-pdf"));
            Tools.Add(new Tool("text2pdf", "fa-file-pdf"));
            Tools.Add(new Tool("extract images", "fa-file-image"));
            Tools.Add(new Tool("extract text", "fa-file-alt"));
            Tools.Add(new Tool("set password", "fa-lock"));
            Tools.Add(new Tool("remove password", "fa-unlock"));
        }
    }
}
