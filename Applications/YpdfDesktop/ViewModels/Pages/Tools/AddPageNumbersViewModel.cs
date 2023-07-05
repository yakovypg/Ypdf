using Avalonia.Threading;
using ExecutionLib.Configuration;
using ExecutionLib.Informing.Aliases;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Infrastructure.Default;
using YpdfDesktop.Models;
using YpdfDesktop.Models.IO;
using YpdfDesktop.Models.Localization;
using YpdfDesktop.ViewModels.Base;
using YpdfLib.Models.Design;
using YpdfLib.Models.Paging;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class AddPageNumbersViewModel : PdfToolViewModel, IFilePathContainer
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectInputFilePathCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectOutputFilePathCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectFontFilePathCommand { get; }

        #endregion

        #region Reactive Properties

        public static double MinFontSize => 6;
        public static double MaxFontSize => 72;

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

        private bool _isFontFilePathSelected = false;
        public bool IsFontFilePathSelected
        {
            get => _isFontFilePathSelected;
            private set => this.RaiseAndSetIfChanged(ref _isFontFilePathSelected, value);
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

        private string? _fontFilePath = null;
        public string? FontFilePath
        {
            get => _fontFilePath;
            private set
            {
                this.RaiseAndSetIfChanged(ref _fontFilePath, value);
                IsFontFilePathSelected = !string.IsNullOrEmpty(value);
            }
        }

        private string _fontFamily = string.Empty;
        public string FontFamily
        {
            get => _fontFamily;
            set => this.RaiseAndSetIfChanged(ref _fontFamily, value);
        }

        private string _fontColor = string.Empty;
        public string FontColor
        {
            get => _fontColor;
            set => this.RaiseAndSetIfChanged(ref _fontColor, value);
        }

        private string _fontEncoding = string.Empty;
        public string FontEncoding
        {
            get => _fontEncoding;
            set => this.RaiseAndSetIfChanged(ref _fontEncoding, value);
        }

        private float _fontSize = DEFAULT_FONT_SIZE;
        public float FontSize
        {
            get => _fontSize;
            set => this.RaiseAndSetIfChanged(ref _fontSize, value);
        }

        private float _fontOpacity = DEFAULT_FONT_OPACITY;
        public float FontOpacity
        {
            get => _fontOpacity;
            set => this.RaiseAndSetIfChanged(ref _fontOpacity, value);
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

        private TabAlignment _horizontalAlignment = TabAlignment.CENTER;
        public TabAlignment HorizontalAlignment
        {
            get => _horizontalAlignment;
            set => this.RaiseAndSetIfChanged(ref _horizontalAlignment, value);
        }

        private VerticalAlignment _verticalAlignment = VerticalAlignment.BOTTOM;
        public VerticalAlignment VerticalAlignment
        {
            get => _verticalAlignment;
            set => this.RaiseAndSetIfChanged(ref _verticalAlignment, value);
        }

        private LocationMode _numberLocationMode = LocationMode.INCREASE_BOTTOM;
        public LocationMode NumberLocationMode
        {
            get => _numberLocationMode;
            set => this.RaiseAndSetIfChanged(ref _numberLocationMode, value);
        }

        private string _textPresenter = string.Empty;
        public string TextPresenter
        {
            get => _textPresenter;
            set => this.RaiseAndSetIfChanged(ref _textPresenter, value);
        }

        private bool _considerLeftPageMargin = true;
        public bool ConsiderLeftPageMargin
        {
            get => _considerLeftPageMargin;
            set => this.RaiseAndSetIfChanged(ref _considerLeftPageMargin, value);
        }

        private bool _considerTopPageMargin = true;
        public bool ConsiderTopPageMargin
        {
            get => _considerTopPageMargin;
            set => this.RaiseAndSetIfChanged(ref _considerTopPageMargin, value);
        }

        private bool _considerRightPageMargin = true;
        public bool ConsiderRightPageMargin
        {
            get => _considerRightPageMargin;
            set => this.RaiseAndSetIfChanged(ref _considerRightPageMargin, value);
        }

        private bool _considerBottomPageMargin = true;
        public bool ConsiderBottomPageMargin
        {
            get => _considerBottomPageMargin;
            set => this.RaiseAndSetIfChanged(ref _considerBottomPageMargin, value);
        }

        private bool _isPageShouldBeIncreased = false;
        public bool IsPageShouldBeIncreased
        {
            get => _isPageShouldBeIncreased;
            set => this.RaiseAndSetIfChanged(ref _isPageShouldBeIncreased, value);
        }

        private string _fillColor = string.Empty;
        public string FillColor
        {
            get => _fillColor;
            set => this.RaiseAndSetIfChanged(ref _fillColor, value);
        }

        private string _shiftContentPages = string.Empty;
        public string ShiftContentPages
        {
            get => _shiftContentPages;
            set => this.RaiseAndSetIfChanged(ref _shiftContentPages, value);
        }

        private float? _horizontalContentShift = null;
        public float? HorizontalContentShift
        {
            get => _horizontalContentShift;
            set => this.RaiseAndSetIfChanged(ref _horizontalContentShift, value);
        }

        private float? _verticalContentShift = null;
        public float? VerticalContentShift
        {
            get => _verticalContentShift;
            set => this.RaiseAndSetIfChanged(ref _verticalContentShift, value);
        }

        private float? _leftPageIncrease = null;
        public float? LeftPageIncrease
        {
            get => _leftPageIncrease;
            set => this.RaiseAndSetIfChanged(ref _leftPageIncrease, value);
        }

        private float? _topPageIncrease = null;
        public float? TopPageIncrease
        {
            get => _topPageIncrease;
            set => this.RaiseAndSetIfChanged(ref _topPageIncrease, value);
        }

        private float? _rightPageIncrease = null;
        public float? RightPageIncrease
        {
            get => _rightPageIncrease;
            set => this.RaiseAndSetIfChanged(ref _rightPageIncrease, value);
        }

        private float? _bottomPageIncrease = null;
        public float? BottomPageIncrease
        {
            get => _bottomPageIncrease;
            set => this.RaiseAndSetIfChanged(ref _bottomPageIncrease, value);
        }

        #endregion

        #region Constants

        private const float DEFAULT_FONT_SIZE = 14;
        private const float DEFAULT_FONT_OPACITY = 1;

        #endregion

        #region Observable Collections

        public ObservableCollection<TabAlignment> HorizontalAlignments { get; }
        public ObservableCollection<VerticalAlignment> VerticalAlignments { get; }
        public ObservableCollection<LocationMode> NumberLocationModes { get; }

        public ObservableCollection<string> FontFamilies { get; }
        public ObservableCollection<string> FontColors { get; }
        public ObservableCollection<string> FontEncodings { get; }
        public ObservableCollection<string> TextPresenters { get; }
        public ObservableCollection<string> FillColors { get; }

        #endregion

        // Constructor for Designer
        public AddPageNumbersViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public AddPageNumbersViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            var horizontalAlignments = EnumsInfo.TabAlignments;
            HorizontalAlignments = new ObservableCollection<TabAlignment>(horizontalAlignments);

            var verticalAlignments = EnumsInfo.VerticalAlignments;
            VerticalAlignments = new ObservableCollection<VerticalAlignment>(verticalAlignments);

            var numberLocationModes = EnumsInfo.LocationModes.Except(new[] { LocationMode.DEFAULT });
            NumberLocationModes = new ObservableCollection<LocationMode>(numberLocationModes);

            var fontFamilies = StandardValues.FontFamilies.Keys;
            FontFamilies = new ObservableCollection<string>(fontFamilies);

            var fontColors = StandardValues.Colors.Keys;
            FontColors = new ObservableCollection<string>(fontColors);

            var fontEncodings = StandardValues.Encodings.Keys;
            FontEncodings = new ObservableCollection<string>(fontEncodings);

            var textPresenters = StandardValues.PageNumberTextPresenters.Keys;
            TextPresenters = new ObservableCollection<string>(textPresenters);

            var fillColors = StandardValues.Colors.Keys;
            FillColors = new ObservableCollection<string>(fillColors);

            ExecuteCommand = ReactiveCommand.Create(Execute);
            ResetCommand = ReactiveCommand.Create(Reset);
            SelectInputFilePathCommand = ReactiveCommand.Create(SelectInputFilePath);
            SelectOutputFilePathCommand = ReactiveCommand.Create(SelectOutputFilePath);
            SelectFontFilePathCommand = ReactiveCommand.Create(SelectFontFilePath);

            SetDefaultItems();
        }

        #region Protected Methods

        protected override void Execute()
        {
            if (!VerifyOutputFilePath())
                return;

            var config = new YpdfConfig()
            {
                PdfTool = "add-page-numbers"
            };

            config.PathsConfig.InputPath = InputFilePath;
            config.PathsConfig.OutputPath = CorrectOutputFilePath(OutputFilePath, "pdf");

            if (IsFontFilePathSelected)
            {
                config.FontInfo.Path = FontFilePath;
                config.FontInfo.Encoding = StandardValues.Encodings[FontEncoding];
            }

            config.FontInfo.Family = StandardValues.FontFamilies[FontFamily];
            config.FontInfo.Color = StandardValues.Colors[FontColor];
            config.FontInfo.Size = FontSize;
            config.FontInfo.Opacity = FontOpacity;

            config.Margin = new Margin(
                LeftMargin ?? 0,
                TopMargin ?? 0,
                RightMargin ?? 0,
                BottomMargin ?? 0
            );

            config.PageNumberStyle.HorizontalAlignment = HorizontalAlignment;
            config.PageNumberStyle.VerticalAlignment = VerticalAlignment;
            config.PageNumberStyle.TextPresenter = StandardValues.PageNumberTextPresenters[TextPresenter];
            config.PageNumberStyle.ConsiderLeftPageMargin = ConsiderLeftPageMargin;
            config.PageNumberStyle.ConsiderTopPageMargin = ConsiderTopPageMargin;
            config.PageNumberStyle.ConsiderRightPageMargin = ConsiderRightPageMargin;
            config.PageNumberStyle.ConsiderBottomPageMargin = ConsiderBottomPageMargin;
            
            if (IsPageShouldBeIncreased)
            {
                config.PageChange.FillColor = StandardValues.Colors[FillColor];
                config.PageNumberStyle.LocationMode = NumberLocationMode;

                if (!string.IsNullOrEmpty(ShiftContentPages))
                {
                    if (!TryParseContentShifts(out PageContentShift[] contentShifts))
                    {
                        string message = $"{SettingsVM.Locale.IncorrectShiftContentPagesMessage}.";
                        MainWindowMessage.ShowWarningDialog(message);
                        return;
                    }

                    Array.ForEach(contentShifts, config.ContentShifts.Add);
                }

                config.PageChange.Increase = new PageIncrease(
                    LeftPageIncrease ?? 0,
                    TopPageIncrease ?? 0,
                    RightPageIncrease ?? 0,
                    BottomPageIncrease ?? 0
                );
            }

            Execute(ToolType.AddPageNumbers, config, true);
        }

        protected override void Reset()
        {
            InputFilePath = string.Empty;
            OutputFilePath = string.Empty;
            FontFilePath = string.Empty;

            FontSize = DEFAULT_FONT_SIZE;
            FontOpacity = DEFAULT_FONT_OPACITY;

            LeftMargin = null;
            TopMargin = null;
            RightMargin = null;
            BottomMargin = null;

            ConsiderLeftPageMargin = true;
            ConsiderTopPageMargin = true;
            ConsiderRightPageMargin = true;
            ConsiderBottomPageMargin = true;
            IsPageShouldBeIncreased = false;

            ShiftContentPages = string.Empty;
            HorizontalContentShift = null;
            VerticalContentShift = null;

            LeftPageIncrease = null;
            TopPageIncrease = null;
            RightPageIncrease = null;
            BottomPageIncrease = null;

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

            InputFilePath = path;
            return true;
        }

        private bool SetFontFilePath(string path)
        {
            if (!File.Exists(path))
                return false;

            FontFilePath = path;
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
            const string initialFileName = "PdfWithPageNumbers";

            _ = DialogProvider.GetOutputFilePath(initialFileName, DialogProvider.PdfFilters).ContinueWith(t =>
            {
                if (t.Result is null || string.IsNullOrEmpty(t.Result))
                    return;

                OutputFilePath = t.Result;
            });
        }

        private void SelectFontFilePath()
        {
            _ = DialogProvider.GetInputFilePaths(false).ContinueWith(t =>
            {
                if (t.Result is null || t.Result.Length == 0)
                    return;

                Dispatcher.UIThread.Post(() => SetFontFilePath(t.Result[0]));
            });
        }

        private bool VerifyOutputFilePath()
        {
            return InformIfIncorrect(IsOutputFilePathSelected, SettingsVM.Locale.SpecifyOutputFilePathMessage);
        }

        private bool TryParseContentShifts(out PageContentShift[] contentShifts)
        {
            string data = $"{ShiftContentPages}:{HorizontalContentShift},{VerticalContentShift}";
            
            try
            {
                contentShifts = PageContentShift.ParseMany(data);
                return true;
            }
            catch
            {
                contentShifts = Array.Empty<PageContentShift>();
                return false;
            }
        }

        private void SetDefaultItems()
        {
            SetDefaultFontFamily();
            SetDefaultFontColor();          
            SetDefaultFontEncoding();
            SetDefaultHorizontalAlignment();
            SetDefaultVerticalAlignment();
            SetDefaultLocationMode();
            SetDefaultTextPresenter();
            SetDefaultFillColor();
        }

        private void SetDefaultFontFamily()
        {
            var timesRoman = StandardValues.FontFamilies.First(t => t.Value == StandardFonts.TIMES_ROMAN);

            if (FontFamilies.Contains(timesRoman.Key))
                FontFamily = timesRoman.Key;
            else if (FontFamilies.Count > 0)
                FontFamily = FontFamilies[0];
            else
                FontFamily = StandardFonts.TIMES_ROMAN.ToString();
        }

        private void SetDefaultFontColor()
        {
            var black = StandardValues.Colors.First(t => t.Value == ColorConstants.BLACK);

            if (FontColors.Contains(black.Key))
                FontColor = black.Key;
            else if (FontColors.Count > 0)
                FontColor = FontColors[0];
            else
                FontColor = ColorConstants.BLACK.ToString() ?? string.Empty;
        }

        private void SetDefaultFontEncoding()
        {
            var identityH = StandardValues.Encodings.First(t => t.Value == PdfEncodings.IDENTITY_H);

            if (FontEncodings.Contains(identityH.Key))
                FontEncoding = identityH.Key;
            else if (FontEncodings.Count > 0)
                FontEncoding = FontEncodings[0];
            else
                FontEncoding = PdfEncodings.IDENTITY_H.ToString();
        }

        private void SetDefaultHorizontalAlignment()
        {
            if (HorizontalAlignments.Contains(TabAlignment.CENTER))
                HorizontalAlignment = TabAlignment.CENTER;
            else if (HorizontalAlignments.Count > 0)
                HorizontalAlignment = HorizontalAlignments[0];
            else
                HorizontalAlignment = TabAlignment.CENTER;
        }

        private void SetDefaultVerticalAlignment()
        {
            if (VerticalAlignments.Contains(VerticalAlignment.BOTTOM))
                VerticalAlignment = VerticalAlignment.BOTTOM;
            else if (VerticalAlignments.Count > 0)
                VerticalAlignment = VerticalAlignments[0];
            else
                VerticalAlignment = VerticalAlignment.BOTTOM;
        }

        private void SetDefaultLocationMode()
        {
            if (NumberLocationModes.Contains(LocationMode.INCREASE_BOTTOM))
                NumberLocationMode = LocationMode.INCREASE_BOTTOM;
            else if (NumberLocationModes.Count > 0)
                NumberLocationMode = NumberLocationModes[0];
            else
                NumberLocationMode = LocationMode.INCREASE_BOTTOM;
        }

        private void SetDefaultTextPresenter()
        {
            var defaultPresenter = StandardValues.PageNumberTextPresenters
                .First(t => t.Value == PageNumberTextPresenter.DefaultPresenter);

            if (TextPresenters.Contains(defaultPresenter.Key))
                TextPresenter = defaultPresenter.Key;
            else if (TextPresenters.Count > 0)
                TextPresenter = TextPresenters[0];
            else if (StandardValues.PageNumberTextPresenters.Count > 0)
                TextPresenter = StandardValues.PageNumberTextPresenters.First().Key;
            else
                TextPresenter = string.Empty;
        }

        private void SetDefaultFillColor()
        {
            var black = StandardValues.Colors.First(t => t.Value == ColorConstants.BLACK);

            if (FillColors.Contains(black.Key))
                FillColor = black.Key;
            else if (FillColors.Count > 0)
                FillColor = FillColors[0];
            else
                FillColor = ColorConstants.BLACK.ToString() ?? string.Empty;
        }

        #endregion
    }
}