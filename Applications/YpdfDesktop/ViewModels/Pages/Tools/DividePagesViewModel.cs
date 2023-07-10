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
    public class DividePagesViewModel : PdfToolViewModel, IFilePathContainer
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectInputFilePathCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectOutputFilePathCommand { get; }
        public ReactiveCommand<Unit, Unit> ApplyCurrentDivisionForSelectedPagesCommand { get; }
        public ReactiveCommand<IList, Unit> SwitchDivisionExecutionToTrueCommand { get; }
        public ReactiveCommand<IList, Unit> SwitchDivisionExecutionToFalseCommand { get; }

        #endregion

        #region Public Properties

        /*
         * If you change the value of PdfPageImageBorderThicknessValue, change it in
         * the DividePagesView.axaml as well (value should be doubled). It is used as
         * a converter parameter in the converter for the PDF page image border width
         * and height
        */
        public static double PdfPageImageBorderThicknessValue => 2;
        public static Thickness PdfPageImageBorderThickness => new(PdfPageImageBorderThicknessValue);

        public bool IsAnyDivisionsSpecified => PdfPageDivisionsInfo.Any(t => t.ExecuteDivision);

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

        private PageDivisionOrientation _currentDivisionOrientation = PageDivisionOrientation.Vertical;
        public PageDivisionOrientation CurrentDivisionOrientation
        {
            get => _currentDivisionOrientation;
            private set
            {
                this.RaiseAndSetIfChanged(ref _currentDivisionOrientation, value);

                IsCurrentDivisionOrientationHorizontal = value == PageDivisionOrientation.Horizontal;
                IsCurrentDivisionOrientationVertical = value == PageDivisionOrientation.Vertical;
            }
        }

        private IPageDivisionInfo? _currentPdfPageDivisionInfo = new PageDivisionInfo(0, 0, 0);
        public IPageDivisionInfo? CurrentPdfPageDivisionInfo
        {
            get => _currentPdfPageDivisionInfo;
            set
            {
                ApplyCurrentDivision(_currentPdfPageDivisionInfo);
                this.RaiseAndSetIfChanged(ref _currentPdfPageDivisionInfo, value);

                CurrentPdfPageWidth = value?.PageWidth ?? 0;
                CurrentPdfPageHeight = value?.PageHeight ?? 0;
                CurrentPdfPageImage = string.Empty; // TODO
                HorizontalDivisionPoint = value?.HorizontalDivisionPoint ?? 0;
                VerticalDivisionPoint = value?.VerticalDivisionPoint ?? 0;
                CurrentDivisionOrientation = value?.Orientation ?? 0;

                UpdateVirtualPdfPageSize();
            }
        }

        private int _horizontalDivisionPoint;
        public int HorizontalDivisionPoint
        {
            get => _horizontalDivisionPoint;
            set
            {
                this.RaiseAndSetIfChanged(ref _horizontalDivisionPoint, value);
                UpdateHorizontalDottedLineMargin();
            }
        }

        private int _verticalDivisionPoint;
        public int VerticalDivisionPoint
        {
            get => _verticalDivisionPoint;
            set
            {
                this.RaiseAndSetIfChanged(ref _verticalDivisionPoint, value);
                UpdateVerticalDottedLineMargin();
            }
        }

        private Thickness _horizontalDottedLineMargin = new(0, 0, 0, 0);
        public Thickness HorizontalDottedLineMargin
        {
            get => _horizontalDottedLineMargin;
            private set => this.RaiseAndSetIfChanged(ref _horizontalDottedLineMargin, value);
        }

        private Thickness _verticalDottedLineMargin = new(0, 0, 0, 0);
        public Thickness VerticalDottedLineMargin
        {
            get => _verticalDottedLineMargin;
            private set => this.RaiseAndSetIfChanged(ref _verticalDottedLineMargin, value);
        }

        private bool _isCurrentDivisionOrientationHorizontal = false;
        public bool IsCurrentDivisionOrientationHorizontal
        {
            get => _isCurrentDivisionOrientationHorizontal;
            private set => this.RaiseAndSetIfChanged(ref _isCurrentDivisionOrientationHorizontal, value);
        }

        private bool _isCurrentDivisionOrientationVertical = true;
        public bool IsCurrentDivisionOrientationVertical
        {
            get => _isCurrentDivisionOrientationVertical;
            private set => this.RaiseAndSetIfChanged(ref _isCurrentDivisionOrientationVertical, value);
        }

        private string _pagesToApllyCurrentDivision = string.Empty;
        public string PagesToApllyCurrentDivision
        {
            get => _pagesToApllyCurrentDivision;
            private set => this.RaiseAndSetIfChanged(ref _pagesToApllyCurrentDivision, value);
        }

        #endregion

        #region Observable Collections

        public ObservableCollection<IPageDivisionInfo> PdfPageDivisionsInfo { get; }
        public ObservableCollection<PageDivisionOrientation> Orientations { get; }

        #endregion

        // Constructor for Designer
        public DividePagesViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public DividePagesViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            PdfPageDivisionsInfo = new ObservableCollection<IPageDivisionInfo>();

            var orientations = Enum.GetValues<PageDivisionOrientation>();
            Orientations = new ObservableCollection<PageDivisionOrientation>(orientations);

            ExecuteCommand = ReactiveCommand.Create(Execute);
            ResetCommand = ReactiveCommand.Create(Reset);
            SelectInputFilePathCommand = ReactiveCommand.Create(SelectInputFilePath);
            SelectOutputFilePathCommand = ReactiveCommand.Create(SelectOutputFilePath);
            ApplyCurrentDivisionForSelectedPagesCommand = ReactiveCommand.Create(ApplyCurrentDivisionForSelectedPages);
            SwitchDivisionExecutionToTrueCommand = ReactiveCommand.Create<IList>(SwitchDivisionExecutionToTrue);
            SwitchDivisionExecutionToFalseCommand = ReactiveCommand.Create<IList>(SwitchDivisionExecutionToFalse);

            SetDefaultItems();
        }

        #region Protected Methods

        protected override void Execute()
        {
            ApplyCurrentDivision(CurrentPdfPageDivisionInfo);

            if (!VerifyOutputFilePath() || !VerifyDivisionsCount())
                return;

            var config = new YpdfConfig()
            {
                PdfTool = "divide"
            };

            config.PathsConfig.InputPath = InputFilePath;
            config.PathsConfig.OutputPath = CorrectOutputFilePath(OutputFilePath, "pdf");

            var divisions = PdfPageDivisionsInfo
                .Where(t => t.ExecuteDivision)
                .Select(t => t.GetDivision())
                .ToArray();

            Array.ForEach(divisions, config.PageDivisions.Add);

            Execute(ToolType.DividePages, config, true);
        }

        protected override void Reset()
        {
            PdfPageDivisionsInfo.Clear();

            InputFilePath = string.Empty;
            OutputFilePath = string.Empty;
            PagesToApllyCurrentDivision = string.Empty;
            CurrentPdfPageDivisionInfo = new PageDivisionInfo(0, 0, 0);

            SetDefaultItems();
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
            const string initialFileName = "DividedPdf";

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

        private bool VerifyDivisionsCount()
        {
            return InformIfIncorrect(IsAnyDivisionsSpecified, SettingsVM.Locale.SpecifyAtLeastOneDivisionMessage);
        }

        private void ApplyCurrentDivisionForSelectedPages()
        {
            if (CurrentPdfPageDivisionInfo is null)
                return;

            string pagesToApllyCurrentDivision = PagesToApllyCurrentDivision.Replace(" ", null);

            if (string.IsNullOrEmpty(pagesToApllyCurrentDivision))
            {
                CurrentPdfPageDivisionInfo.ExecuteDivision = true;
                ApplyCurrentDivision(CurrentPdfPageDivisionInfo);
                return;
            }

            PageRange[] ranges;

            try
            {
                ranges = AbstractParser.ParseMany(pagesToApllyCurrentDivision, ',', PageRange.Parse);
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

            var divisions = PdfPageDivisionsInfo.Where(t => pages.Contains(t.PageNumber));

            foreach (var division in divisions)
            {
                division.ExecuteDivision = true;
                ApplyCurrentDivision(division);
            }
        }

        private void ApplyCurrentDivision(IPageDivisionInfo? division)
        {
            if (division is null)
                return;

            division.Orientation = CurrentDivisionOrientation;
            division.HorizontalDivisionPoint = HorizontalDivisionPoint;
            division.VerticalDivisionPoint = VerticalDivisionPoint;
        }

        private void SwitchDivisionExecutionToTrue(IList divisionsList)
        {
            SwitchDivisionExecution(divisionsList, t => true);
        }

        private void SwitchDivisionExecutionToFalse(IList divisionsList)
        {
            SwitchDivisionExecution(divisionsList, t => false);
        }

        private void SwitchDivisionExecution(IList divisionsList, Func<bool, bool> func)
        {
            IEnumerable<IPageDivisionInfo> divisions = divisionsList.Cast<IPageDivisionInfo>();

            foreach (var division in divisions)
            {
                division.ExecuteDivision = func.Invoke(division.ExecuteDivision);
            }
        }

        private bool TryLoadPdfConfig(string path)
        {
            try
            {
                Rectangle[] pageSizes = PdfInfo.GetPageSizes(path);

                if (pageSizes.Length == 0)
                    return false;

                PdfPageDivisionsInfo.Clear();

                for (int i = 0; i < pageSizes.Length; ++i)
                {
                    Rectangle currPageSize = pageSizes[i];
                    float currPageWidth = currPageSize.GetWidth();
                    float currPageHeight = currPageSize.GetHeight();

                    var divisionInfo = new PageDivisionInfo(i + 1, currPageWidth, currPageHeight);
                    PdfPageDivisionsInfo.Add(divisionInfo);
                }
            }
            catch
            {
                return false;
            }

            CurrentPdfPageDivisionInfo = PdfPageDivisionsInfo[0];
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

            UpdateHorizontalDottedLineMargin();
            UpdateVerticalDottedLineMargin();
        }

        private void UpdateHorizontalDottedLineMargin()
        {
            double p = CurrentPdfPageHeight != 0
                ? HorizontalDivisionPoint / CurrentPdfPageHeight
                : 0;

            double bottomMargin = CurrentVirtualPdfPageHeight * p;

            if (bottomMargin >= CurrentVirtualPdfPageHeight)
                bottomMargin = CurrentVirtualPdfPageHeight - 1;

            HorizontalDottedLineMargin = new Thickness(0, 0, 0, bottomMargin);
        }

        private void UpdateVerticalDottedLineMargin()
        {
            double p = CurrentPdfPageWidth != 0
                ? VerticalDivisionPoint / CurrentPdfPageWidth
                : 0;

            double leftMargin = CurrentVirtualPdfPageWidth * p;

            if (leftMargin >= CurrentVirtualPdfPageWidth)
                leftMargin = CurrentVirtualPdfPageWidth - 1;

            VerticalDottedLineMargin = new Thickness(leftMargin, 0, 0, 0);
        }

        private void SetDefaultItems()
        {
            SetDefaultDivisionOrientation();
        }

        private void SetDefaultDivisionOrientation()
        {
            if (Orientations.Contains(PageDivisionOrientation.Vertical))
                CurrentDivisionOrientation = PageDivisionOrientation.Vertical;
            else if (Orientations.Count > 0)
                CurrentDivisionOrientation = Orientations[0];
            else
                CurrentDivisionOrientation = PageDivisionOrientation.Vertical;
        }

        #endregion
    }
}
