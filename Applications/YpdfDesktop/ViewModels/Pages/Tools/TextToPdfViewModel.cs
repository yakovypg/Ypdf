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
using YpdfDesktop.ViewModels.Base;
using YpdfLib.Models.Design;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class TextToPdfViewModel : PdfToolViewModel, IFilePathContainer
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

        private TextAlignment _textAlignment = TextAlignment.LEFT;
        public TextAlignment TextAlignment
        {
            get => _textAlignment;
            set => this.RaiseAndSetIfChanged(ref _textAlignment, value);
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

        private string _pageSize = string.Empty;
        public string PageSize
        {
            get => _pageSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _pageSize, value);
                IsPageSizeCustom = value == DefaultLocales.English.Custom;
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

        private bool _isPageSizeCustom = false;
        public bool IsPageSizeCustom
        {
            get => _isPageSizeCustom;
            private set => this.RaiseAndSetIfChanged(ref _isPageSizeCustom, value);
        }

        #endregion

        #region Constants

        private const float DEFAULT_FONT_SIZE = 14;
        private const float DEFAULT_FONT_OPACITY = 1;
        private const int DEFAULT_PAGE_WIDTH = 2480;
        private const int DEFAULT_PAGE_HEIGHT = 3508;

        #endregion

        #region Observable Collections

        public ObservableCollection<TextAlignment> TextAlignments { get; }
        public ObservableCollection<string> FontFamilies { get; }
        public ObservableCollection<string> FontColors { get; }
        public ObservableCollection<string> FontEncodings { get; }
        public ObservableCollection<string> PageSizes { get; }

        #endregion

        // Constructor for Designer
        public TextToPdfViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public TextToPdfViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            var textAlignments = EnumsInfo.TextAlignments;
            TextAlignments = new ObservableCollection<TextAlignment>(textAlignments);

            var fontFamilies = StandardValues.FontFamilies.Keys;
            FontFamilies = new ObservableCollection<string>(fontFamilies);

            var fontColors = StandardValues.Colors.Keys;
            FontColors = new ObservableCollection<string>(fontColors);

            var fontEncodings = StandardValues.Encodings.Keys;
            FontEncodings = new ObservableCollection<string>(fontEncodings);

            var pageSizes = StandardValues.PageSizes.Keys;
            PageSizes = new ObservableCollection<string>(pageSizes);
            PageSizes.Insert(0, DefaultLocales.English.Custom!);

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
                PdfTool = "text-to-pdf"
            };

            config.PathsConfig.InputPath = InputFilePath;
            config.PathsConfig.OutputPath = CorrectOutputFilePath(OutputFilePath, "pdf");

            if (IsFontFilePathSelected)
            {
                config.FontInfo.Path = FontFilePath;
                config.FontInfo.Encoding = StandardValues.Encodings[FontEncoding];
            }

            config.TextPageParameters.TextAlignment = TextAlignment;
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

            if (IsPageSizeCustom)
            {
                config.PageChange.NewWidth = PageWidth;
                config.PageChange.NewHeight = PageHeight;
            }
            else
            {
                config.PageChange.PageSize = StandardValues.PageSizes[PageSize];
            }

            Execute(ToolType.TextToPdf, config, true);
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

            PageWidth = DEFAULT_PAGE_WIDTH;
            PageHeight = DEFAULT_PAGE_HEIGHT;

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

            if (!IsPathToTxt(path))
            {
                string message = $"{SettingsVM.Locale.FileNotTxtMessage}.";
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
            _ = DialogProvider.GetTxtFilePaths(false).ContinueWith(t =>
            {
                if (t.Result is null || t.Result.Length == 0)
                    return;

                Dispatcher.UIThread.Post(() => SetInputFilePath(t.Result[0]));
            });
        }

        private void SelectOutputFilePath()
        {
            const string initialFileName = "PdfFromTxt";

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

        private void SetDefaultItems()
        {
            SetDefaultFontFamily();
            SetDefaultFontColor();          
            SetDefaultTextAlignment();
            SetDefaultFontEncoding();
            SetDefaultPageSize();
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

        private void SetDefaultTextAlignment()
        {
            if (TextAlignments.Contains(TextAlignment.LEFT))
                TextAlignment = TextAlignment.LEFT;
            else if (TextAlignments.Count > 0)
                TextAlignment = TextAlignments[0];
            else
                TextAlignment = TextAlignment.LEFT;
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

        private void SetDefaultPageSize()
        {
            var pageSizeA4 = StandardValues.PageSizes.First(t => t.Value == iText.Kernel.Geom.PageSize.A4);

            if (PageSizes.Contains(pageSizeA4.Key))
                PageSize = pageSizeA4.Key;
            else if (PageSizes.Count > 0)
                PageSize = PageSizes[^1];
            else
                PageSize = iText.Kernel.Geom.PageSize.A4.ToString();
        }

        #endregion
    }
}