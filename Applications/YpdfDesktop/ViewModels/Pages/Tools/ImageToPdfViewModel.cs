using Avalonia.Threading;
using DynamicData;
using ExecutionLib.Configuration;
using ExecutionLib.Informing.Aliases;
using iText.IO.Font;
using iText.Layout.Properties;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Infrastructure.Default;
using YpdfDesktop.Models;
using YpdfDesktop.Models.IO;
using YpdfDesktop.Models.Localization;
using YpdfDesktop.ViewModels.Base;
using YpdfLib.Models.Design;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class ImageToPdfViewModel : PdfToolViewModel, IFilePathCollectionContainer, ILazyLocalizable
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteSelectedInputFilesCommand { get; }
        public ReactiveCommand<Unit, Unit> AddInputFilesCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectOutputFilePathCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateSelectedInputFilesCountCommand { get; }

        #endregion

        #region Reactive Properties

        private string _outputFilePath = string.Empty;
        public string OutputFilePath
        {
            get => _outputFilePath;
            private set
            {
                this.RaiseAndSetIfChanged(ref _outputFilePath, value);
                IsOutputFilePathSelected = !string.IsNullOrEmpty(value);
            }
        }

        private bool _isOutputFilePathSelected = false;
        public bool IsOutputFilePathSelected
        {
            get => _isOutputFilePathSelected;
            private set => this.RaiseAndSetIfChanged(ref _isOutputFilePathSelected, value);
        }

        private bool _isAnyInputFilesAdded = false;
        public bool IsAnyInputFilesAdded
        {
            get => _isAnyInputFilesAdded;
            private set => this.RaiseAndSetIfChanged(ref _isAnyInputFilesAdded, value);
        }

        private bool _isAnyInputFilesSelected = false;
        public bool IsAnyInputFilesSelected
        {
            get => _isAnyInputFilesSelected;
            private set => this.RaiseAndSetIfChanged(ref _isAnyInputFilesSelected, value);
        }

        private int _inputFilesCount = 0;
        public int InputFilesCount
        {
            get => _inputFilesCount;
            private set
            {
                this.RaiseAndSetIfChanged(ref _inputFilesCount, value);
                IsAnyInputFilesAdded = value > 0;
            }
        }

        private int _selectedInputFilesCount = 0;
        public int SelectedInputFilesCount
        {
            get => _selectedInputFilesCount;
            private set
            {
                this.RaiseAndSetIfChanged(ref _selectedInputFilesCount, value);
                IsAnyInputFilesSelected = value > 0;
            }
        }

        private IList _selectedInputFiles = new List<InputFile>();
        public IList SelectedInputFiles
        {
            get => _selectedInputFiles;
            set => this.RaiseAndSetIfChanged(ref _selectedInputFiles, value);
        }

        private bool _autoincreaseSize = true;
        public bool AutoincreaseSize
        {
            get => _autoincreaseSize;
            set => this.RaiseAndSetIfChanged(ref _autoincreaseSize, value);
        }

        private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.CENTER;
        public HorizontalAlignment HorizontalAlignment
        {
            get => _horizontalAlignment;
            set => this.RaiseAndSetIfChanged(ref _horizontalAlignment, value);
        }

        private float? _leftMargin = null;
        public float? LeftMargin
        {
            get => _leftMargin;
            set => this.RaiseAndSetIfChanged(ref _leftMargin, value);
        }

        private float? _topMargin = null;
        public float? TopMargin
        {
            get => _topMargin;
            set => this.RaiseAndSetIfChanged(ref _topMargin, value);
        }

        private float? _rightMargin = null;
        public float? RightMargin
        {
            get => _rightMargin;
            set => this.RaiseAndSetIfChanged(ref _rightMargin, value);
        }

        private float? _bottomMargin = null;
        public float? BottomMargin
        {
            get => _bottomMargin;
            set => this.RaiseAndSetIfChanged(ref _bottomMargin, value);
        }

        private string _pageSize = string.Empty;
        public string PageSize
        {
            get => _pageSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _pageSize, value);
                IsPageSizeAuto = value == (SettingsVM.Locale.Auto ?? DefaultLocales.English.Auto);
                IsPageSizeCustom = value == (SettingsVM.Locale.Custom ?? DefaultLocales.English.Custom);
            }
        }

        private int _pageWidth = DEFAULT_PAGE_WIDTH;
        public int PageWidth
        {
            get => _pageWidth;
            set => this.RaiseAndSetIfChanged(ref _pageWidth, value);
        }

        private int _pageHeight = DEFAULT_PAGE_HEIGHT;
        public int PageHeight
        {
            get => _pageHeight;
            set => this.RaiseAndSetIfChanged(ref _pageHeight, value);
        }

        private bool _isPageSizeAuto = false;
        public bool IsPageSizeAuto
        {
            get => _isPageSizeAuto;
            private set => this.RaiseAndSetIfChanged(ref _isPageSizeAuto, value);
        }

        private bool _isPageSizeCustom = false;
        public bool IsPageSizeCustom
        {
            get => _isPageSizeCustom;
            private set => this.RaiseAndSetIfChanged(ref _isPageSizeCustom, value);
        }

        #endregion

        #region Constants

        private const int DEFAULT_PAGE_WIDTH = 2480;
        private const int DEFAULT_PAGE_HEIGHT = 3508;
        private const int AUTO_PAGE_SIZE_INDEX = 0;
        private const int CUSTOM_PAGE_SIZE_INDEX = 1;

        #endregion

        #region Observable Collections

        public ObservableCollection<InputFile> InputFiles { get; }
        public ObservableCollection<HorizontalAlignment> HorizontalAlignments { get; }
        public ObservableCollection<string> PageSizes { get; }

        #endregion

        // Constructor for Designer
        public ImageToPdfViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public ImageToPdfViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            InputFiles = new ObservableCollection<InputFile>();
            InputFiles.CollectionChanged += InputFilesCollectionChanged;

            var horizontalAlignments = EnumsInfo.HorizontalAlignments;
            HorizontalAlignments = new ObservableCollection<HorizontalAlignment>(horizontalAlignments);

            var pageSizes = StandardValues.PageSizes.Keys;
            PageSizes = new ObservableCollection<string>(pageSizes);
            PageSizes.Insert(AUTO_PAGE_SIZE_INDEX, SettingsVM.Locale.Auto ?? DefaultLocales.English.Auto!);
            PageSizes.Insert(CUSTOM_PAGE_SIZE_INDEX, SettingsVM.Locale.Custom ?? DefaultLocales.English.Custom!);

            ExecuteCommand = ReactiveCommand.Create(Execute);
            ResetCommand = ReactiveCommand.Create(Reset);
            DeleteSelectedInputFilesCommand = ReactiveCommand.Create(DeleteSelectedInputFiles);
            AddInputFilesCommand = ReactiveCommand.Create(AddInputFiles);
            SelectOutputFilePathCommand = ReactiveCommand.Create(SelectOutputFilePath);
            UpdateSelectedInputFilesCountCommand = ReactiveCommand.Create(UpdateSelectedInputFilesCount);

            SetDefaultItems();
        }

        #region Public Methods

        public void Localize()
        {
            int selectedPageSizeIndex = PageSizes.IndexOf(PageSize);

            bool isSelectedPageSizeChanging = selectedPageSizeIndex == AUTO_PAGE_SIZE_INDEX
                || selectedPageSizeIndex == CUSTOM_PAGE_SIZE_INDEX;

            bool isLocalized = false;
            
            if (!string.IsNullOrEmpty(SettingsVM.Locale.Auto)
                && LocalizeCollectionItem(PageSizes, AUTO_PAGE_SIZE_INDEX, SettingsVM.Locale.Auto))
            {
                this.RaisePropertyChanged(nameof(PageSizes));
                isLocalized = true;
            }

            if (!string.IsNullOrEmpty(SettingsVM.Locale.Custom)
                && LocalizeCollectionItem(PageSizes, CUSTOM_PAGE_SIZE_INDEX, SettingsVM.Locale.Custom))
            {
                this.RaisePropertyChanged(nameof(PageSizes));
                isLocalized = true;
            }

            if (isSelectedPageSizeChanging && isLocalized)
                PageSize = PageSizes[selectedPageSizeIndex];
        }

        #endregion

        #region Protected Methods

        protected override void Execute()
        {
            if (!VerifyOutputFilePath())
                return;

            var config = new YpdfConfig()
            {
                PdfTool = "image-to-pdf"
            };

            config.PathsConfig.OutputPath = CorrectOutputFilePath(OutputFilePath, "pdf");

            foreach (var inputFile in InputFiles)
                config.PathsConfig.FilePaths.Add(inputFile.File.FullName);

            config.ImagePageParameters.AutoIncreaseSize = AutoincreaseSize;
            config.ImagePageParameters.HorizontalAlignment = HorizontalAlignment;

            config.Margin = new Margin(
                LeftMargin ?? 0,
                TopMargin ?? 0,
                RightMargin ?? 0,
                BottomMargin ?? 0
            );

            if (!IsPageSizeAuto)
            {
                if (IsPageSizeCustom)
                {
                    config.PageChange.NewWidth = PageWidth;
                    config.PageChange.NewHeight = PageHeight;
                }
                else
                {
                    config.PageChange.PageSize = StandardValues.PageSizes[PageSize];
                }   
            }

            Execute(ToolType.ImageToPdf, config, true);
        }

        protected override void Reset()
        {
            InputFiles.Clear();
            OutputFilePath = string.Empty;

            AutoincreaseSize = true;

            LeftMargin = null;
            TopMargin = null;
            RightMargin = null;
            BottomMargin = null;

            PageWidth = DEFAULT_PAGE_WIDTH;
            PageHeight = DEFAULT_PAGE_HEIGHT;

            SetDefaultItems();
        }

        #endregion

        #region Private Methods

        private void InputFilesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            InputFilesCount = InputFiles.Count;
        }

        private void DeleteSelectedInputFiles()
        {
            InputFiles.RemoveMany(SelectedInputFiles.Cast<InputFile>());
        }

        private void UpdateSelectedInputFilesCount()
        {
            SelectedInputFilesCount = SelectedInputFiles.Count;
        }

        private void AddInputFiles()
        {
            _ = DialogProvider.GetImageFilePaths(true).ContinueWith(t =>
            {
                if (t.Result is null || t.Result.Length == 0)
                    return;

                foreach (string path in t.Result)
                {
                    try
                    {
                        InputFiles.Add(new InputFile(path));
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.UIThread.Post(() => MainWindowMessage.ShowErrorDialog(ex.Message));
                    }
                }
            });
        }

        private void SelectOutputFilePath()
        {
            const string initialFileName = "PdfFromImages";

            _ = DialogProvider.GetOutputFilePath(initialFileName, DialogProvider.PdfFilters).ContinueWith(t =>
            {
                if (t.Result is null || string.IsNullOrEmpty(t.Result))
                    return;

                OutputFilePath = t.Result;
            });
        }

        private bool VerifyOutputFilePath()
        {
            return InformIfIncorrect(IsOutputFilePathSelected, SettingsVM.Locale.SpecifyOutputFilePathMessage);
        }

        private void SetDefaultItems()
        {
            SetDefaultHorizontalAlignment();
            SetDefaultPageSize();
        }

        private void SetDefaultHorizontalAlignment()
        {
            if (HorizontalAlignments.Contains(HorizontalAlignment.CENTER))
                HorizontalAlignment = HorizontalAlignment.CENTER;
            else if (HorizontalAlignments.Count > 0)
                HorizontalAlignment = HorizontalAlignments[0];
            else
                HorizontalAlignment = HorizontalAlignment.CENTER;
        }

        private void SetDefaultPageSize()
        {
            string autoSize = SettingsVM.Locale.Auto ?? DefaultLocales.English.Auto!;

            if (PageSizes.Contains(autoSize))
                PageSize = autoSize;
            else if (PageSizes.Count > 0)
                PageSize = PageSizes[^1];
            else
                PageSize = autoSize;
        }

        #endregion
    }
}
