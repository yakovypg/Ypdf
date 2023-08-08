using Avalonia;
using Avalonia.Threading;
using ExecutionLib.Configuration;
using ExecutionLib.Informing.Aliases;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
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
using YpdfLib.Models.Geometry;
using YpdfLib.Models.Geometry.Transformation;
using YpdfLib.Models.Enumeration;
using YpdfLib.Models.Parsing;
using YpdfLib.Models.Paging;
using iText.Kernel.Geom;
using Avalonia.Controls;
using YpdfDesktop.Infrastructure.Converters;
using System.Globalization;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class AddWatermarkViewModel : PdfToolViewModel, IFilePathContainer
    {
        // TODO Add watermark border support
        // TODO Fix bounded rectangle
        
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

        private Rect _watermarkGhostTextBlockBounds;
        public Rect WatermarkGhostTextBlockBounds
        {
            get => _watermarkGhostTextBlockBounds;
            set
            {
                this.RaiseAndSetIfChanged(ref _watermarkGhostTextBlockBounds, value);
                
                WatermarkGhostTextWidth = value.Width;
                WatermarkGhostTextHeight = value.Height;
            }
        }

        private double _watermarkGhostTextWidth;
        public double WatermarkGhostTextWidth
        {
            get => _watermarkGhostTextWidth;
            set
            {
                if (value > 0 && double.IsFinite(value))
                    this.RaiseAndSetIfChanged(ref _watermarkGhostTextWidth, value);
            }
        }

        private double _watermarkGhostTextHeight;
        public double WatermarkGhostTextHeight
        {
            get => _watermarkGhostTextHeight;
            set
            {
                if (value > 0 && double.IsFinite(value))
                    this.RaiseAndSetIfChanged(ref _watermarkGhostTextHeight, value);
            }
        }

        private Rect _watermarkBounds;
        public Rect WatermarkBounds
        {
            get => _watermarkBounds;
            set
            {
                this.RaiseAndSetIfChanged(ref _watermarkBounds, value);
                UpdateWatermarkTopLeftMargin();
            }
        }

        private Rect _watermarkTextBounds;
        public Rect WatermarkTextBounds
        {
            get => _watermarkTextBounds;
            set
            {
                this.RaiseAndSetIfChanged(ref _watermarkTextBounds, value);

                UpdateWatermarkBounds();
                UpdateRotationCenter();
            }
        }

        private FloatRectangle _watermarkRotatedTextBounds;
        public FloatRectangle WatermarkRotatedTextBounds
        {
            get => _watermarkRotatedTextBounds;
            set => this.RaiseAndSetIfChanged(ref _watermarkRotatedTextBounds, value);
        }

        private double _watermarkTextTopLeftX;
        public double WatermarkTextTopLeftX
        {
            get => _watermarkTextTopLeftX;
            set
            {
                this.RaiseAndSetIfChanged(ref _watermarkTextTopLeftX, value);
                UpdateWatermarkTopLeftMargin();
            }
        }

        private double _watermarkTextTopLeftY;
        public double WatermarkTextTopLeftY
        {
            get => _watermarkTextTopLeftY;
            set
            {
                this.RaiseAndSetIfChanged(ref _watermarkTextTopLeftY, value);
                UpdateWatermarkTopLeftMargin();
            }
        }

        private Thickness _watermarkTextTopLeftMargin = new();
        public Thickness WatermarkTextTopLeftMargin
        {
            get => _watermarkTextTopLeftMargin;
            private set => this.RaiseAndSetIfChanged(ref _watermarkTextTopLeftMargin, value);
        }

        private Thickness _watermarkBoundsTopLeftMargin = new();
        public Thickness WatermarkBoundsTopLeftMargin
        {
            get => _watermarkBoundsTopLeftMargin;
            private set => this.RaiseAndSetIfChanged(ref _watermarkBoundsTopLeftMargin, value);
        }

        private double _rotationCenterX;
        public double RotationCenterX
        {
            get => _rotationCenterX;
            private set => this.RaiseAndSetIfChanged(ref _rotationCenterX, value);
        }

        private double _rotationCenterY;
        public double RotationCenterY
        {
            get => _rotationCenterY;
            private set => this.RaiseAndSetIfChanged(ref _rotationCenterY, value);
        }

        private float _firstPdfPageWidth;
        public float FirstPdfPageWidth
        {
            get => _firstPdfPageWidth;
            private set => this.RaiseAndSetIfChanged(ref _firstPdfPageWidth, value);
        }

        private float _firstPdfPageHeight;
        public float FirstPdfPageHeight
        {
            get => _firstPdfPageHeight;
            private set => this.RaiseAndSetIfChanged(ref _firstPdfPageHeight, value);
        }

        private string _firstPdfPageImage = string.Empty;
        public string FirstPdfPageImage
        {
            get => _firstPdfPageImage;
            private set => this.RaiseAndSetIfChanged(ref _firstPdfPageImage, value);
        }

        private string _selectedPages = string.Empty;
        public string SelectedPages
        {
            get => _selectedPages;
            private set => this.RaiseAndSetIfChanged(ref _selectedPages, value);
        }

        private double _rotationAngle;
        public double RotationAngle
        {
            get => _rotationAngle;
            set
            {
                this.RaiseAndSetIfChanged(ref _rotationAngle, value);
                UpdateWatermarkBounds();
            }
        }

        private string _watermarkText = DEFAULT_WATERMARK_TEXT;
        public string WatermarkText
        {
            get => _watermarkText;
            set => this.RaiseAndSetIfChanged(ref _watermarkText, value);
        }

        private string _fontFamily = string.Empty;
        public string FontFamily
        {
            get => _fontFamily;
            set => this.RaiseAndSetIfChanged(ref _fontFamily, value);
        }

        private Color _fontColor = ColorConstants.BLACK;
        public Color FontColor
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

        private bool _makeAsAnnotation = false;
        public bool MakeAsAnnotation
        {
            get => _makeAsAnnotation;
            set => this.RaiseAndSetIfChanged(ref _makeAsAnnotation, value);
        }

        private bool _isWatermarkBoundsVisible = true;
        public bool IsWatermarkBoundsVisible
        {
            get => _isWatermarkBoundsVisible;
            set => this.RaiseAndSetIfChanged(ref _isWatermarkBoundsVisible, value);
        }

        private bool _isWatermarkTextBoundsVisible = true;
        public bool IsWatermarkTextBoundsVisible
        {
            get => _isWatermarkTextBoundsVisible;
            set => this.RaiseAndSetIfChanged(ref _isWatermarkTextBoundsVisible, value);
        }

        private bool _isWatermarkRotatedTextBoundsVisible = true;
        public bool IsWatermarkRotatedTextBoundsVisible
        {
            get => _isWatermarkRotatedTextBoundsVisible;
            set => this.RaiseAndSetIfChanged(ref _isWatermarkRotatedTextBoundsVisible, value);
        }

        #endregion

        #region Constants

        private const float DEFAULT_FONT_SIZE = 14;
        private const float DEFAULT_FONT_OPACITY = 1;
        private const string DEFAULT_WATERMARK_TEXT = "My Watermark";

        private const bool USE_EXHAUSTIVE_WATERMARK_AREA = true;
        private const float EXHAUSTIVE_WATERMARK_AREA_WIDTH = 10000;
        private const float EXHAUSTIVE_WATERMARK_AREA_HEIGHT = 10000;

        #endregion

        #region Observable Collections

        public ObservableCollection<IPageInfo> PdfPages { get; }

        public ObservableCollection<string> FontFamilies { get; }
        public ObservableCollection<string> FontColors { get; }
        public ObservableCollection<string> FontEncodings { get; }

        #endregion

        // Constructor for Designer
        public AddWatermarkViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public AddWatermarkViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            PdfPages = new ObservableCollection<IPageInfo>();

            var fontFamilies = StandardValues.FontFamilies.Keys;
            FontFamilies = new ObservableCollection<string>(fontFamilies);

            var fontColors = StandardValues.Colors.Keys;
            FontColors = new ObservableCollection<string>(fontColors);

            var fontEncodings = StandardValues.Encodings.Keys;
            FontEncodings = new ObservableCollection<string>(fontEncodings);
            
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

            bool isPdfToolPrepared = MakeAsAnnotation
                ? PrepareAddWatermarkAnnotationTool(out YpdfConfig config)
                : PrepareAddWatermarkTool(out config);
            
            if (!isPdfToolPrepared)
                return;

            Execute(ToolType.AddWatermark, config, true);
        }

        protected override void Reset()
        {
            InputFilePath = string.Empty;
            OutputFilePath = string.Empty;
            FontFilePath = string.Empty;

            WatermarkTextTopLeftX = 0;
            WatermarkTextTopLeftY = 0;

            FirstPdfPageWidth = 0;
            FirstPdfPageHeight = 0;
            FirstPdfPageImage = string.Empty;
            
            SelectedPages = string.Empty;
            RotationAngle = 0;

            WatermarkText = DEFAULT_WATERMARK_TEXT;
            FontSize = DEFAULT_FONT_SIZE;
            FontOpacity = DEFAULT_FONT_OPACITY;

            MakeAsAnnotation = false;
            IsWatermarkBoundsVisible = true;
            IsWatermarkTextBoundsVisible = true;
            IsWatermarkRotatedTextBoundsVisible = true;

            SetDefaultItems();
        }

        #endregion

        #region Private Methods

        private bool PrepareAddWatermarkTool(out YpdfConfig config)
        {
            config = new YpdfConfig()
            {
                PdfTool = "add-watermark"
            };

            if (!ConfigurePages(config))
                return false;

            ConfigureFont(config);
            ConfigurePaths(config);
            ConfigureText(config);

            config.Watermark.LowerLeftPoint = CalculateWatermarkLowerLeftPoint();

            FloatSize size = CalculateWatermarkSize();
            config.Watermark.SetWidth(size.Width);
            config.Watermark.SetHeight(size.Height);

            return true;
        }

        private bool PrepareAddWatermarkAnnotationTool(out YpdfConfig config)
        {
            config = new YpdfConfig()
            {
                PdfTool = "add-watermark-annotation"
            };

            if (!ConfigurePages(config))
                return false;

            ConfigureFont(config);
            ConfigurePaths(config);
            ConfigureText(config);

            config.Watermark.LowerLeftPoint = CalculateWatermarkAnnotationLowerLeftPoint();

            FloatSize size = CalculateWatermarkAnnotationSize();
            config.Watermark.SetWidth(size.Width);
            config.Watermark.SetHeight(size.Height);

            return true;
        }

        private void ConfigureFont(YpdfConfig config)
        {
            if (IsFontFilePathSelected)
            {
                config.FontInfo.Path = FontFilePath;
                config.FontInfo.Encoding = StandardValues.Encodings[FontEncoding];
            }

            config.FontInfo.Family = StandardValues.FontFamilies[FontFamily];
            config.FontInfo.Color = FontColor;
            config.FontInfo.Size = FontSize;
            config.FontInfo.Opacity = FontOpacity;
        }

        private void ConfigurePaths(YpdfConfig config)
        {
            config.PathsConfig.InputPath = InputFilePath;
            config.PathsConfig.OutputPath = CorrectOutputFilePath(OutputFilePath, "pdf");
        }

        private void ConfigureText(YpdfConfig config)
        {
            config.Watermark.Text = WatermarkText;

            if (RotationAngle != 0)
                config.RotationAngle = Convert.ToInt32(RotationAngle);

            if (USE_EXHAUSTIVE_WATERMARK_AREA)
            {
                config.WatermarkTextAllocator.TextAlignment = TextAlignment.LEFT;
                config.WatermarkTextAllocator.TextHorizontalAlignment = HorizontalAlignment.LEFT;
                config.WatermarkTextAllocator.TextContainerVerticalAlignment = VerticalAlignment.BOTTOM;
            }
        }

        private bool ConfigurePages(YpdfConfig config)
        {
            if (string.IsNullOrEmpty(SelectedPages))
            {
                config.Pages.Add(new PageRange(1, PdfPages.Count));
                return true;
            }
            
            if (!TryParsePageRanges(SelectedPages, out PageRange[] pageRanges))
            {
                string message = $"{SettingsVM.Locale.IncorrectPagesMessage}.";
                MainWindowMessage.ShowErrorDialog(message);
                return false;
            }

            Array.ForEach(pageRanges, config.Pages.Add);

            return true;
        }

        private FloatPoint CalculateWatermarkLowerLeftPoint()
        {
            double x = WatermarkTextBounds.BottomLeft.X;
            double y = FirstPdfPageHeight - WatermarkTextBounds.BottomLeft.Y;

            return FloatPoint.FromDoubleCoordinates(x, y);
        }

        private FloatPoint CalculateWatermarkAnnotationLowerLeftPoint()
        {
            double x = WatermarkBounds.BottomLeft.X;
            double y = FirstPdfPageHeight - WatermarkBounds.BottomLeft.Y;

            return FloatPoint.FromDoubleCoordinates(x, y);
        }

        private FloatSize CalculateWatermarkSize()
        {
            return USE_EXHAUSTIVE_WATERMARK_AREA
                ? new FloatSize(EXHAUSTIVE_WATERMARK_AREA_WIDTH, EXHAUSTIVE_WATERMARK_AREA_HEIGHT)
                : WatermarkRotatedTextBounds.Size;
        }

        private FloatSize CalculateWatermarkAnnotationSize()
        {
            return USE_EXHAUSTIVE_WATERMARK_AREA
                ? new FloatSize(EXHAUSTIVE_WATERMARK_AREA_WIDTH, EXHAUSTIVE_WATERMARK_AREA_HEIGHT)
                : FloatSize.FromDoubleCoordinates(WatermarkBounds.Width, WatermarkBounds.Height);
        }

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
            const string initialFileName = "PdfWithWatermark";

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

        private bool TryLoadPdfConfig(string path)
        {
            try
            {
                Rectangle[] pageSizes = PdfInfo.GetPageSizes(path);

                if (pageSizes.Length == 0)
                    return false;

                PdfPages.Clear();

                for (int i = 0; i < pageSizes.Length; ++i)
                {
                    Rectangle currPageSize = pageSizes[i];
                    float currPageWidth = currPageSize.GetWidth();
                    float currPageHeight = currPageSize.GetHeight();

                    var pageInfo = new PageInfo(i + 1, currPageWidth, currPageHeight);
                    PdfPages.Add(pageInfo);
                }
            }
            catch
            {
                return false;
            }

            FirstPdfPageWidth = PdfPages[0].Width;
            FirstPdfPageHeight = PdfPages[0].Height;
            FirstPdfPageImage = string.Empty; // TODO

            UpdateWatermarkTopLeftMargin();

            return true;
        }

        private void UpdateWatermarkBounds()
        {
            FloatPoint textTopLeft = FloatPoint.FromDoubleCoordinates(
                WatermarkTextBounds.TopLeft.X,
                WatermarkTextBounds.TopLeft.Y
            );

            FloatPoint textBottomRight = FloatPoint.FromDoubleCoordinates(
                WatermarkTextBounds.BottomRight.X,
                WatermarkTextBounds.BottomRight.Y
            );

            FloatPoint rotationCenter = FloatPoint.FromDoubleCoordinates(
                WatermarkTextBounds.BottomLeft.X,
                WatermarkTextBounds.BottomLeft.Y
            );

            Angle angle = Angle.FromDegrees(-RotationAngle);
            RotationRectangleTransform rectangleTransform = new(angle, rotationCenter);

            FloatRectangle textBounds = new(textTopLeft, textBottomRight);
            FloatRectangle rotatedTextBounds = rectangleTransform.Transform(textBounds);
            FloatRectangle watermarkBounds = BoundedRectangle.FromRectangle(rotatedTextBounds);

            Avalonia.Point boundsTopLeft = new(watermarkBounds.TopLeft.X, watermarkBounds.TopLeft.Y);
            Avalonia.Point boundsBottomRight = new( watermarkBounds.BottomRight.X, watermarkBounds.BottomRight.Y);

            WatermarkBounds = new Rect(boundsTopLeft, boundsBottomRight);
            WatermarkRotatedTextBounds = rotatedTextBounds;
        }

        private void UpdateRotationCenter()
        {
            RotationCenterX = -WatermarkTextBounds.Width / 2.0;
            RotationCenterY = WatermarkTextBounds.Height / 2.0;
        }

        private void UpdateWatermarkTopLeftMargin()
        {           
            double textLeftMargin = WatermarkTextTopLeftX;
            double textTopMargin = WatermarkTextTopLeftY;

            double boundsLeftMargin = _watermarkBounds.TopLeft.X;
            double boundsTopMargin = _watermarkBounds.TopLeft.Y;

            WatermarkTextTopLeftMargin = new Thickness(textLeftMargin, textTopMargin, 0, 0);
            WatermarkBoundsTopLeftMargin = new Thickness(boundsLeftMargin, boundsTopMargin, 0, 0);
        }

        private void SetDefaultItems()
        {
            SetDefaultFontFamily();
            SetDefaultFontColor();          
            SetDefaultFontEncoding();
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
            Color color = ColorConstants.BLACK;

            var converter = new ColorValueConverter();
            string colorKey = converter.Convert(color) as string ?? string.Empty;

            if (FontColors.Contains(colorKey))
                FontColor = color;
            else if (FontColors.Count > 0)
                FontColor = converter.ConvertBack(FontColors[0]) as Color ?? color;
            else
                FontColor = color;
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

        private static bool TryParsePageRanges(string data, out PageRange[] pageRanges)
        {
            try
            {
                pageRanges = AbstractParser.ParseMany(data, ',', PageRange.Parse);
                return true;
            }
            catch
            {
                pageRanges = Array.Empty<PageRange>();
                return false;
            }
        }

        #endregion
    }
}
