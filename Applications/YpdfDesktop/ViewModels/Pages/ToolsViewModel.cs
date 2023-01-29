﻿using Avalonia.Media;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Infrastructure.Default;
using YpdfDesktop.Models;
using YpdfDesktop.Models.Localization;
using YpdfDesktop.Models.Themes;
using YpdfDesktop.ViewModels.Pages.Tools;

namespace YpdfDesktop.ViewModels.Pages
{
    public class ToolsViewModel : ViewModelBase
    {
        #region Commands

        public ReactiveCommand<ToolType, Unit> ChangeToolAffiliationToFavoritesCommand { get; }
        public ReactiveCommand<ToolType, Unit> ShowToolPageCommand { get; }
        public ReactiveCommand<Unit, Unit> HideAllToolsPagesCommand { get; }

        #endregion

        #region ViewModels

        public SettingsViewModel SettingsVM { get; }

        public SplitViewModel SplitVM { get; }
        public MergeViewModel MergeVM { get; }
        public CompressViewModel CompressVM { get; }

        #endregion

        #region Reactive Properties

        private bool _isSplitViewVisible = false;
        public bool IsSplitViewVisible
        {
            get => _isSplitViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isSplitViewVisible, value);
        }

        private bool _isMergeViewVisible = false;
        public bool IsMergeViewVisible
        {
            get => _isMergeViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isMergeViewVisible, value);
        }

        private bool _isCompressViewVisible = false;
        public bool IsCompressViewVisible
        {
            get => _isCompressViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isCompressViewVisible, value);
        }

        private bool _isHandlePagesViewVisible = false;
        public bool IsHandlePagesViewVisible
        {
            get => _isHandlePagesViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isHandlePagesViewVisible, value);
        }

        private bool _isCropPagesViewVisible = false;
        public bool IsCropPagesViewVisible
        {
            get => _isCropPagesViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isCropPagesViewVisible, value);
        }

        private bool _isDividePagesViewVisible = false;
        public bool IsDividePagesViewVisible
        {
            get => _isDividePagesViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isDividePagesViewVisible, value);
        }

        private bool _isAddPageNumbersViewVisible = false;
        public bool IsAddPageNumbersViewVisible
        {
            get => _isAddPageNumbersViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isAddPageNumbersViewVisible, value);
        }

        private bool _isAddWatermarkViewVisible = false;
        public bool IsAddWatermarkViewVisible
        {
            get => _isAddWatermarkViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isAddWatermarkViewVisible, value);
        }

        private bool _isRemoveWatermarkViewVisible = false;
        public bool IsRemoveWatermarkViewVisible
        {
            get => _isRemoveWatermarkViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isRemoveWatermarkViewVisible, value);
        }

        private bool _isImageToPdfViewVisible = false;
        public bool IsImageToPdfViewVisible
        {
            get => _isImageToPdfViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isImageToPdfViewVisible, value);
        }

        private bool _isTextToPdfViewVisible = false;
        public bool IsTextToPdfViewVisible
        {
            get => _isTextToPdfViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isTextToPdfViewVisible, value);
        }

        private bool _isExtractImagesViewVisible = false;
        public bool IsExtractImagesViewVisible
        {
            get => _isExtractImagesViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isExtractImagesViewVisible, value);
        }

        private bool _isExtractTextViewVisible = false;
        public bool IsExtractTextViewVisible
        {
            get => _isExtractTextViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isExtractTextViewVisible, value);
        }

        private bool _isSetPasswordViewVisible = false;
        public bool IsSetPasswordViewVisible
        {
            get => _isSetPasswordViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isSetPasswordViewVisible, value);
        }

        private bool _isRemovePasswordViewVisible = false;
        public bool IsRemovePasswordViewVisible
        {
            get => _isRemovePasswordViewVisible;
            private set => this.RaiseAndSetIfChanged(ref _isRemovePasswordViewVisible, value);
        }

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

            SplitVM = new SplitViewModel(settingsVM);
            MergeVM = new MergeViewModel();
            CompressVM = new CompressViewModel();

            Tools = tools ?? DefaultTools.Get(settingsVM.Locale, settingsVM.Theme);
            FavoriteTools = favoriteTools ?? new ObservableCollection<Tool>();

            ChangeToolAffiliationToFavoritesCommand = ReactiveCommand.Create<ToolType>(ChangeToolAffiliationToFavorites);
            ShowToolPageCommand = ReactiveCommand.Create<ToolType>(ShowToolPage);
            HideAllToolsPagesCommand = ReactiveCommand.Create(HideAllToolsPages);
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

            Tool? tool = Tools.FirstOrDefault(t => t.Type == toolType);

            if (tool is null)
                return;

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

            if (!Tools.Any(t => t.Type == toolType))
                return;

            switch (toolType)
            {
                case ToolType.Split: IsSplitViewVisible = true; break;
                case ToolType.Merge: IsMergeViewVisible = true; break;
                case ToolType.Compress: IsCompressViewVisible = true; break;
                case ToolType.HandlePages: IsHandlePagesViewVisible = true; break;
                case ToolType.CropPages: IsCropPagesViewVisible = true; break;
                case ToolType.DividePages: IsDividePagesViewVisible = true; break;
                case ToolType.AddPageNumbers: IsAddPageNumbersViewVisible = true; break;
                case ToolType.AddWatermark: IsAddWatermarkViewVisible = true; break;
                case ToolType.RemoveWatermark: IsRemoveWatermarkViewVisible = true; break;
                case ToolType.ImageToPdf: IsImageToPdfViewVisible = true; break;
                case ToolType.TextToPdf: IsTextToPdfViewVisible = true; break;
                case ToolType.ExtractImages: IsExtractImagesViewVisible = true; break;
                case ToolType.ExtractText: IsExtractTextViewVisible = true; break;
                case ToolType.SetPassword: IsSetPasswordViewVisible = true; break;
                case ToolType.RemovePassword: IsRemovePasswordViewVisible = true; break;
                default: break;
            }
        }

        private void HideAllToolsPages()
        {
            IsSplitViewVisible = false;
            IsMergeViewVisible = false;
            IsCompressViewVisible = false;
            IsHandlePagesViewVisible = false;
            IsCropPagesViewVisible = false;
            IsDividePagesViewVisible = false;
            IsAddPageNumbersViewVisible = false;
            IsAddWatermarkViewVisible = false;
            IsRemoveWatermarkViewVisible = false;
            IsImageToPdfViewVisible = false;
            IsTextToPdfViewVisible = false;
            IsExtractImagesViewVisible = false;
            IsExtractTextViewVisible = false;
            IsSetPasswordViewVisible = false;
            IsRemovePasswordViewVisible = false;
        }

        #endregion
    }
}