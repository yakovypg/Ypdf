using Avalonia;
using Avalonia.Threading;
using ExecutionLib.Configuration;
using ExecutionLib.Informing.Aliases;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Infrastructure.Default;
using YpdfDesktop.Models;
using YpdfDesktop.Models.IO;
using YpdfDesktop.ViewModels.Base;
using YpdfDesktop.Models.Paging;
using YpdfLib.Models.Design;
using YpdfLib.Informing;
using YpdfLib.Models.Enumeration;
using YpdfLib.Models.Parsing;
using YpdfLib.Models.Paging;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class CropPagesViewModel : PdfToolViewModel, IFilePathContainer
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectInputFilePathCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectOutputFilePathCommand { get; }
        public ReactiveCommand<Unit, Unit> ApplyCurrentCroppingForSelectedPagesCommand { get; }
        public ReactiveCommand<IList, Unit> SwitchCroppingExecutionToTrueCommand { get; }
        public ReactiveCommand<IList, Unit> SwitchCroppingExecutionToFalseCommand { get; }

        #endregion

        #region Public Properties

        /*
         * If you change the value of PdfPageImageBorderThicknessValue, change it in
         * the CropPagesView.axaml as well (value should be doubled). It is used as
         * a converter parameter in the converter for the PDF page image border width
         * and height
        */
        public static double PdfPageImageBorderThicknessValue => 2;
        public static Thickness PdfPageImageBorderThickness => new(PdfPageImageBorderThicknessValue);

        public bool IsAnyCroppingsSpecified => PdfPageCroppingsInfo.Any(t => t.ExecuteCropping);

        #endregion

        #region Reactive Properties

        private bool _isInputFilePathSelected = false;
        public bool IsInputFilePathSelected
        {
            get => _isInputFilePathSelected;
            private set => this.RaiseAndSetIfChanged(ref _isInputFilePathSelected, value);
        }

        private bool _isOutputFilePathSelected = false;
        public bool IsOutputFilePathSelected
        {
            get => _isOutputFilePathSelected;
            private set => this.RaiseAndSetIfChanged(ref _isOutputFilePathSelected, value);
        }

        private string _inputFilePath = string.Empty;
        public string InputFilePath
        {
            get => _inputFilePath;
            private set
            {
                this.RaiseAndSetIfChanged(ref _inputFilePath, value);
                IsInputFilePathSelected = !string.IsNullOrEmpty(value);
            }
        }

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

        private Rect _virtualPdfPageBounds;
        public Rect VirtualPdfPageBounds
        {
            get => _virtualPdfPageBounds;
            set
            {
                this.RaiseAndSetIfChanged(ref _virtualPdfPageBounds, value);
                UpdateVirtualPdfPageSize();
            }
        }

        private double _currentVirtualPdfPageWidth;
        public double CurrentVirtualPdfPageWidth
        {
            get => _currentVirtualPdfPageWidth;
            private set => this.RaiseAndSetIfChanged(ref _currentVirtualPdfPageWidth, value);
        }

        private double _currentVirtualPdfPageHeight;
        public double CurrentVirtualPdfPageHeight
        {
            get => _currentVirtualPdfPageHeight;
            private set => this.RaiseAndSetIfChanged(ref _currentVirtualPdfPageHeight, value);
        }

        private float _currentPdfPageWidth;
        public float CurrentPdfPageWidth
        {
            get => _currentPdfPageWidth;
            private set => this.RaiseAndSetIfChanged(ref _currentPdfPageWidth, value);
        }

        private float _currentPdfPageHeight;
        public float CurrentPdfPageHeight
        {
            get => _currentPdfPageHeight;
            private set => this.RaiseAndSetIfChanged(ref _currentPdfPageHeight, value);
        }

        private string _currentPdfPageImage = string.Empty;
        public string CurrentPdfPageImage
        {
            get => _currentPdfPageImage;
            private set => this.RaiseAndSetIfChanged(ref _currentPdfPageImage, value);
        }

        private IPageCroppingInfo? _currentPdfPageCroppingInfo = new PageCroppingInfo(0, 0, 0);
        public IPageCroppingInfo? CurrentPdfPageCroppingInfo
        {
            get => _currentPdfPageCroppingInfo;
            set
            {
                ApplyCurrentCropping(_currentPdfPageCroppingInfo);
                this.RaiseAndSetIfChanged(ref _currentPdfPageCroppingInfo, value);

                CurrentPdfPageWidth = value?.PageWidth ?? 0;
                CurrentPdfPageHeight = value?.PageHeight ?? 0;
                CurrentPdfPageImage = string.Empty; // TODO
                UpperRightPointX = value?.UpperRightPointX ?? 0;
                UpperRightPointY = value?.UpperRightPointY ?? 0;
                LowerLeftPointX = value?.LowerLeftPointX ?? 0;
                LowerLeftPointY = value?.LowerLeftPointY ?? 0;

                UpdateVirtualPdfPageSize();
            }
        }

        private float _lowerLeftPointX;
        public float LowerLeftPointX
        {
            get => _lowerLeftPointX;
            set
            {
                this.RaiseAndSetIfChanged(ref _lowerLeftPointX, value);
                UpdateLowerLeftPointXDottedLineMargin();
            }
        }

        private float _lowerLeftPointY;
        public float LowerLeftPointY
        {
            get => _lowerLeftPointY;
            set
            {
                this.RaiseAndSetIfChanged(ref _lowerLeftPointY, value);
                UpdateLowerLeftPointYDottedLineMargin();
            }
        }

        private float _upperRightPointX;
        public float UpperRightPointX
        {
            get => _upperRightPointX;
            set
            {
                this.RaiseAndSetIfChanged(ref _upperRightPointX, value);
                UpdateUpperRightPointXDottedLineMargin();
            }
        }

        private float _upperRightPointY;
        public float UpperRightPointY
        {
            get => _upperRightPointY;
            set
            {
                this.RaiseAndSetIfChanged(ref _upperRightPointY, value);
                UpdateUpperRightPointYDottedLineMargin();
            }
        }

        private Thickness _lowerLeftPointXDottedLineMargin = new(0, 0, 0, 0);
        public Thickness LowerLeftPointXDottedLineMargin
        {
            get => _lowerLeftPointXDottedLineMargin;
            private set => this.RaiseAndSetIfChanged(ref _lowerLeftPointXDottedLineMargin, value);
        }

        private Thickness _lowerLeftPointYDottedLineMargin = new(0, 0, 0, 0);
        public Thickness LowerLeftPointYDottedLineMargin
        {
            get => _lowerLeftPointYDottedLineMargin;
            private set => this.RaiseAndSetIfChanged(ref _lowerLeftPointYDottedLineMargin, value);
        }

        private Thickness _upperRightPointXDottedLineMargin = new(0, 0, 0, 0);
        public Thickness UpperRightPointXDottedLineMargin
        {
            get => _upperRightPointXDottedLineMargin;
            private set => this.RaiseAndSetIfChanged(ref _upperRightPointXDottedLineMargin, value);
        }

        private Thickness _upperRightPointYDottedLineMargin = new(0, 0, 0, 0);
        public Thickness UpperRightPointYDottedLineMargin
        {
            get => _upperRightPointYDottedLineMargin;
            private set => this.RaiseAndSetIfChanged(ref _upperRightPointYDottedLineMargin, value);
        }

        private string _pagesToApllyCurrentCropping = string.Empty;
        public string PagesToApllyCurrentCropping
        {
            get => _pagesToApllyCurrentCropping;
            private set => this.RaiseAndSetIfChanged(ref _pagesToApllyCurrentCropping, value);
        }

        #endregion

        #region Observable Collections

        public ObservableCollection<IPageCroppingInfo> PdfPageCroppingsInfo { get; }

        #endregion

        // Constructor for Designer
        public CropPagesViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public CropPagesViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            PdfPageCroppingsInfo = new ObservableCollection<IPageCroppingInfo>();

            ExecuteCommand = ReactiveCommand.Create(Execute);
            ResetCommand = ReactiveCommand.Create(Reset);
            SelectInputFilePathCommand = ReactiveCommand.Create(SelectInputFilePath);
            SelectOutputFilePathCommand = ReactiveCommand.Create(SelectOutputFilePath);
            ApplyCurrentCroppingForSelectedPagesCommand = ReactiveCommand.Create(ApplyCurrentCroppingForSelectedPages);
            SwitchCroppingExecutionToTrueCommand = ReactiveCommand.Create<IList>(SwitchCroppingExecutionToTrue);
            SwitchCroppingExecutionToFalseCommand = ReactiveCommand.Create<IList>(SwitchCroppingExecutionToFalse);
        }

        #region Protected Methods

        protected override void Execute()
        {
            ApplyCurrentCropping(CurrentPdfPageCroppingInfo);

            if (!VerifyOutputFilePath() || !VerifyCropsCount())
                return;

            var config = new YpdfConfig()
            {
                PdfTool = "crop"
            };

            config.PathsConfig.InputPath = InputFilePath;
            config.PathsConfig.OutputPath = CorrectOutputFilePath(OutputFilePath, "pdf");

            var croppings = PdfPageCroppingsInfo
                .Where(t => t.ExecuteCropping)
                .Select(t => t.GetCropping())
                .ToArray();

            Array.ForEach(croppings, config.PageCroppings.Add);

            Execute(ToolType.CropPages, config, true);
        }

        protected override void Reset()
        {
            PdfPageCroppingsInfo.Clear();

            InputFilePath = string.Empty;
            OutputFilePath = string.Empty;
            PagesToApllyCurrentCropping = string.Empty;
            CurrentPdfPageCroppingInfo = new PageCroppingInfo(0, 0, 0);
        }

        #endregion

        #region Private Methods

        bool IFilePathContainer.SetFilePath(string path)
        {
            return SetInputFilePath(path);
        }

        private bool SetInputFilePath(string path)
        {
            if (!File.Exists(path))
                return false;

            if (!IsPathToPdf(path))
            {
                string message = $"{SettingsVM.Locale.FileNotPdfMessage}.";
                MainWindowMessage.ShowErrorDialog(message);
                return false;
            }

            if (!TryLoadPdfConfig(path))
            {
                string message = $"{SettingsVM.Locale.FailedToLoadFileMessage}.";
                MainWindowMessage.ShowErrorDialog(message);
                return false;
            }

            InputFilePath = path;
            return true;
        }

        private void SelectInputFilePath()
        {
            _ = DialogProvider.GetPdfFilePaths(false).ContinueWith(t =>
            {
                if (t.Result is null || t.Result.Length == 0)
                    return;

                Dispatcher.UIThread.Post(() => SetInputFilePath(t.Result[0]));
            });
        }

        private void SelectOutputFilePath()
        {
            const string initialFileName = "CroppedPdf";

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

        private bool VerifyCropsCount()
        {
            return InformIfIncorrect(IsAnyCroppingsSpecified, SettingsVM.Locale.SpecifyAtLeastOneCroppingMessage);
        }

        private void ApplyCurrentCroppingForSelectedPages()
        {
            if (CurrentPdfPageCroppingInfo is null)
                return;

            string pagesToApllyCurrentCropping = PagesToApllyCurrentCropping.Replace(" ", null);

            if (string.IsNullOrEmpty(pagesToApllyCurrentCropping))
            {
                CurrentPdfPageCroppingInfo.ExecuteCropping = true;
                ApplyCurrentCropping(CurrentPdfPageCroppingInfo);
                return;
            }

            PageRange[] ranges;

            try
            {
                ranges = AbstractParser.ParseMany(pagesToApllyCurrentCropping, ',', PageRange.Parse);
            }
            catch
            {
                string message = $"{SettingsVM.Locale.IncorrectPagesMessage}.";
                MainWindowMessage.ShowErrorDialog(message);
                return;
            }

            List<int> pages = new();

            foreach (var range in ranges)
            {
                pages.AddRange(range.Items);
            }

            var croppings = PdfPageCroppingsInfo.Where(t => pages.Contains(t.PageNumber));

            foreach (var cropping in croppings)
            {
                cropping.ExecuteCropping = true;
                ApplyCurrentCropping(cropping);
            }
        }

        private void ApplyCurrentCropping(IPageCroppingInfo? cropping)
        {
            if (cropping is null)
                return;

            cropping.UpperRightPointX = UpperRightPointX;
            cropping.UpperRightPointY = UpperRightPointY;
            cropping.LowerLeftPointX = LowerLeftPointX;
            cropping.LowerLeftPointY = LowerLeftPointY;
        }

        private void SwitchCroppingExecutionToTrue(IList croppingsList)
        {
            SwitchCroppingExecution(croppingsList, t => true);
        }

        private void SwitchCroppingExecutionToFalse(IList croppingsList)
        {
            SwitchCroppingExecution(croppingsList, t => false);
        }

        private static void SwitchCroppingExecution(IList croppingsList, Func<bool, bool> func)
        {
            IEnumerable<IPageCroppingInfo> croppings = croppingsList.Cast<IPageCroppingInfo>();

            foreach (var cropping in croppings)
            {
                cropping.ExecuteCropping = func.Invoke(cropping.ExecuteCropping);
            }
        }

        private bool TryLoadPdfConfig(string path)
        {
            try
            {
                Rectangle[] pageSizes = PdfInfo.GetPageSizes(path);

                if (pageSizes.Length == 0)
                    return false;

                PdfPageCroppingsInfo.Clear();

                for (int i = 0; i < pageSizes.Length; ++i)
                {
                    Rectangle currPageSize = pageSizes[i];
                    float currPageWidth = currPageSize.GetWidth();
                    float currPageHeight = currPageSize.GetHeight();

                    var croppingInfo = new PageCroppingInfo(i + 1, currPageWidth, currPageHeight);
                    PdfPageCroppingsInfo.Add(croppingInfo);
                }
            }
            catch
            {
                return false;
            }

            CurrentPdfPageCroppingInfo = PdfPageCroppingsInfo[0];
            return true;
        }

        private void UpdateVirtualPdfPageSize()
        {
            double maxVirtualPdfPageWidth = VirtualPdfPageBounds.Width
                - PdfPageImageBorderThickness.Left
                - PdfPageImageBorderThickness.Right;

            double maxVirtualPdfPageHeight = VirtualPdfPageBounds.Height
                - PdfPageImageBorderThickness.Top
                - PdfPageImageBorderThickness.Bottom;

            if (CurrentPdfPageHeight >= CurrentPdfPageWidth && CurrentPdfPageHeight != 0)
            {
                CurrentVirtualPdfPageHeight = Math.Min(
                    maxVirtualPdfPageHeight,
                    CurrentPdfPageHeight);

                CurrentVirtualPdfPageWidth = Math.Min(
                    maxVirtualPdfPageWidth,
                    CurrentVirtualPdfPageHeight * CurrentPdfPageWidth / CurrentPdfPageHeight);
            }
            else if (CurrentPdfPageWidth >= CurrentPdfPageHeight && CurrentPdfPageWidth != 0)
            {
                CurrentVirtualPdfPageWidth = Math.Min(
                    maxVirtualPdfPageWidth,
                    CurrentPdfPageWidth);

                CurrentVirtualPdfPageHeight = Math.Min(
                    maxVirtualPdfPageHeight,
                    CurrentVirtualPdfPageWidth * CurrentPdfPageHeight / CurrentPdfPageWidth);
            }
            else
            {
                CurrentVirtualPdfPageWidth = maxVirtualPdfPageWidth;
                CurrentVirtualPdfPageHeight = maxVirtualPdfPageHeight;
            }

            UpdateUpperRightPointXDottedLineMargin();
            UpdateUpperRightPointYDottedLineMargin();
            UpdateLowerLeftPointXDottedLineMargin();
            UpdateLowerLeftPointYDottedLineMargin();
        }

        private void UpdateUpperRightPointXDottedLineMargin()
        {
            double p = CurrentPdfPageWidth != 0
                ? UpperRightPointX / CurrentPdfPageWidth
                : 0;

            double leftMargin = CurrentVirtualPdfPageWidth * p;

            if (leftMargin >= CurrentVirtualPdfPageWidth)
                leftMargin = CurrentVirtualPdfPageWidth - 1;

            UpperRightPointXDottedLineMargin = new Thickness(leftMargin, 0, 0, 0);
        }
        
        private void UpdateUpperRightPointYDottedLineMargin()
        {
            double p = CurrentPdfPageHeight != 0
                ? UpperRightPointY / CurrentPdfPageHeight
                : 0;

            double bottomMargin = CurrentVirtualPdfPageHeight * p;

            if (bottomMargin >= CurrentVirtualPdfPageHeight)
                bottomMargin = CurrentVirtualPdfPageHeight - 1;

            UpperRightPointYDottedLineMargin = new Thickness(0, 0, 0, bottomMargin);
        }

        private void UpdateLowerLeftPointXDottedLineMargin()
        {
            double p = CurrentPdfPageWidth != 0
                ? LowerLeftPointX / CurrentPdfPageWidth
                : 0;

            double leftMargin = CurrentVirtualPdfPageWidth * p;

            if (leftMargin >= CurrentVirtualPdfPageWidth)
                leftMargin = CurrentVirtualPdfPageWidth - 1;

            LowerLeftPointXDottedLineMargin = new Thickness(leftMargin, 0, 0, 0);
        }

        private void UpdateLowerLeftPointYDottedLineMargin()
        {
            double p = CurrentPdfPageHeight != 0
                ? LowerLeftPointY / CurrentPdfPageHeight
                : 0;

            double bottomMargin = CurrentVirtualPdfPageHeight * p;

            if (bottomMargin >= CurrentVirtualPdfPageHeight)
                bottomMargin = CurrentVirtualPdfPageHeight - 1;

            LowerLeftPointYDottedLineMargin = new Thickness(0, 0, 0, bottomMargin);
        }

        #endregion
    }
}
