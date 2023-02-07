using ExecutionLib.Configuration;
using ExecutionLib.Informing.Aliases;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using Mono.Options;
using YpdfLib.Models.Design;
using YpdfLib.Models.Enumeration;
using YpdfLib.Models.Geometry;
using YpdfLib.Models.Paging;

namespace ypdf.Parsing
{
    internal class ArgsParser : IArgsParser, IEquatable<ArgsParser?>
    {
        private YpdfConfig _config;
        private readonly OptionSet _options;

        public Action<TextWriter> OptionDescriptionsWriter => _options.WriteOptionDescriptions;

        public ArgsParser()
        {
            _config = new YpdfConfig();

            _options = new OptionSet()
            {
                { "h|help", "show help.", t => _config.ShowHelp = t is not null },
                { "v|version", "show version.", t => _config.ShowVersion = t is not null },
                { "g|guide", "show guide.", t => _config.ShowGuide = t is not null },

                { "show-config=", "show global config.", (bool t) => _config.ShowGlobalConfig = t },
                { "save-config=", "save global config.", (bool t) => _config.SaveGlobalConfig = t },
                { "reset-config=", "reset global config.", (bool t) => _config.ResetGlobalConfig = t },

                { "a|angle=", "the pages angle.", (int t) => _config.RotationAngle = t },
                { "split-part=", "the splitting part size in bytes.", t => _config.SplittingPartSize = ParseSplittingPartSize(t) },

                { "m|margin=", "the margin (M or H,V or L,T,R,B).", t => _config.Margin = Margin.Parse(t) },
                { "page-order=", "the order of the pages (for example 5,3,2,1,4 or 5,3-1,4).", t => _config.PageOrder = PageOrder.Parse(t) },

                { "p|page=", "the page number or the page range (for example 1 or 3-5).", t => _config.Pages.Add(PageRange.Parse(t)) },
                { "r|rotation=", "the page rotation (format: Pages:Angle ; for example 1:-90 or 1,3-5:90).", t => Array.ForEach(PageRotation.ParseMany(t), _config.PageRotations.Add) },
                { "cropping=", "the page cropping (format: Pages:LowerLeftPoint,UpperRightPoint ; for example 1:(5,5),(40,60) or 1,3-5:(5,5),(40,60)).", t => Array.ForEach(PageCropping.ParseMany(t), _config.PageCroppings.Add) },
                { "division=", "the page division (format: Pages:Orientation,CenterOffset ; for example 1:horizontal or 1,3-5:vertical,10).", t => Array.ForEach(PageDivision.ParseMany(t), _config.PageDivisions.Add) },
                { "content-shift=", "the content shift (format: Pages:Horizontal,Vertical ; for example 1:-50,0 or 1,3-5:10,15).", t => Array.ForEach(PageContentShift.ParseMany(t), _config.ContentShifts.Add) },

                { "i|input-file=", "the input file path.", t => _config.PathsConfig.InputPath = t },
                { "o|output-file=", "the output file path.", t => _config.PathsConfig.OutputPath = t },
                { "O|output-dir=", "the directory where the output files will be placed.", t => _config.PathsConfig.OutputDirectory = t },
                { "f|file=", "the file path.", t => _config.PathsConfig.FilePaths.Add(t) },
                { "files-from=", "the directory from which the files should be taken.", t => _config.PathsConfig.DirectoriesForGettingFiles.Add(t) },
                { "file-pattern=", "the pattern to match files taken from the directory specified by the --files-from parameter.", t => _config.PathsConfig.FileSearchPattern = t },

                { "python-alias=", "the alias or the path where python can be found.", t => _config.GlobalConfig.PythonAlias = t },

                { "font-path=", "the font path.", t => _config.FontInfo.Path = t },
                { "font-encoding=", $"the font encoding ({StandardValues.EncodingNames}).", t => _config.FontInfo.Encoding = ParseEncoding(t) },
                { "s|font-size=", "the font size.", (float t) => _config.FontInfo.Size = t },
                { "font-family=", $"the font family ({StandardValues.FontFamilyNames}).", t => _config.FontInfo.Family = ParseFontFamily(t) },
                { "font-opacity=", "the font opacity.", (float t) => _config.FontInfo.Opacity = t },
                { "c|font-color=", $"the font color ({StandardValues.ColorNames}).", t => _config.FontInfo.Color = ParseColor(t) },

                { "S|image-size-factor=", "the image size factor (from 0 to 1).", (float t) => _config.ImageCompression.SizeFactor = t },
                { "Q|image-quality=", "the image quality factor (from 0 to 1).", (float t) => _config.ImageCompression.QualityFactor = t },
                { "E|image-extension=", "the image new extension (for example png or jpg).", t => _config.ImageCompression.Extension = t },
                { "W|image-width=", "the image new width.", (int t) => _config.ImageCompression.Width = t },
                { "H|image-height=", "the image new height.", (int t) => _config.ImageCompression.Height = t },
                { "check-compression-validity=", "is compression validity check will be performed (true/false).", (bool t) => _config.ImageCompression.CheckCompressionValidity = t },

                { "F|from=", "the source page number.", (int t) => _config.PageMovement.PageToMove = t },
                { "T|to=", "the destination page number.", (int t) => _config.PageMovement.NewPosition = t },

                { "w|watermark-text=", "the watermark text.", t => _config.Watermark.Text = t },
                { "watermark-pos=", "the lower left point of the watermark (x;y).", t => _config.Watermark.LowerLeftPoint = FloatPoint.Parse(t) },
                { "watermark-width=", "the watermark object width.", (float t) => _config.Watermark.SetWidth(t) },
                { "watermark-height=", "the watermark object height.", (float t) => _config.Watermark.SetHeight(t) },
                { "watermark-x-translation=", "the shift of the origin along the X-axis for the watermark annotation.", (float t) => _config.Watermark.XTranslation = t },
                { "watermark-y-translation=", "the shift of the origin along the Y-axis for the watermark annotation.", (float t) => _config.Watermark.YTranslation = t },

                { "password=", "the common PDF document password (sets the same user password and owner password).", t => _config.PdfPassword.SetCommonPassword(t) },
                { "user-password=", "the user PDF document password.", t => _config.PdfPassword.UserPassword = t },
                { "owner-password=", "the owner PDF document password.", t => _config.PdfPassword.OwnerPassword = t },
                { "encryption-algorithm=", $"the encryption algorithm ({StandardValues.EncryptionAlgorithmNames}).", t => _config.PdfPassword.EncryptionAlgorithm = ParseEncryptionAlgorithm(t) },

                { "page-width=", "the page width.", (int t) => _config.PageChange.NewWidth = t },
                { "page-height=", "the page height.", (int t) => _config.PageChange.NewHeight = t },
                { "page-size=", $"the page size (WxH or {StandardValues.PageSizeNames}).", t => _config.PageChange.PageSize = ParsePageSize(t) },
                { "page-increase=", "the page increase (L,T,R,B).", t => _config.PageChange.Increase = PageIncrease.Parse(t) },
                { "fill-color=", $"the fill color ({StandardValues.ColorNames}).", t => _config.PageChange.FillColor = ParseColor(t) },

                { "num-location-mode=", $"the page number location mode ({EnumsInfo.LocationModeValues}).", t => _config.PageNumberStyle.LocationMode = ParseLocationMode(t) },
                { "h-num-alignment=", $"the horizontal page number alignment ({EnumsInfo.TabAlignmentValues}).", t => _config.PageNumberStyle.HorizontalAlignment = ParseTabAlignment(t) },
                { "v-num-alignment=", $"the vertical page number alignment ({EnumsInfo.VerticalAlignmentValues}).", t => _config.PageNumberStyle.VerticalAlignment = ParseVerticalAlignment(t) },
                { "num-presenter=", $"the page number presenter ({StandardValues.PageNumberTextPresenterNames}).", t => _config.PageNumberStyle.TextPresenter = ParsePageNumberTextPresenter(t) },
                { "left-page-margin=", "is left page margin considered (true/false).", (bool t) => _config.PageNumberStyle.ConsiderLeftPageMargin = t },
                { "top-page-margin=", "is top page margin considered (true/false).", (bool t) => _config.PageNumberStyle.ConsiderTopPageMargin = t },
                { "right-page-margin=", "is right page margin considered (true/false).", (bool t) => _config.PageNumberStyle.ConsiderRightPageMargin = t },
                { "bottom-page-margin=", "is bottom page margin considered (true/false).", (bool t) => _config.PageNumberStyle.ConsiderBottomPageMargin = t },

                { "image-autoincrease-size=", "is image autoinsteasing mode enabled (true/false).", (bool t) => _config.ImagePageParameters.AutoIncreaseSize = t },
                { "image-h-alignment=", $"the image horizantal alignment ({EnumsInfo.HorizontalAlignmentValues}).", t => _config.ImagePageParameters.HorizontalAlignment = ParseHorizontalAlignment(t) },

                { "A|text-alignment=", $"the text alignment ({EnumsInfo.TextAlignmentValues}).", t => _config.TextPageParameters.TextAlignment = ParseTextAlignment(t) }
            };
        }

        public bool TryParse(string[] args, out YpdfConfig config)
        {
            return TryParse(args, out config, out _);
        }

        public bool TryParse(string[] args, out YpdfConfig config, out Exception? exception)
        {
            _config = new YpdfConfig();

            try
            {
                List<string> extraArgs = _options.Parse(args);

                int processingToolsCount = extraArgs.Count(t => !t.StartsWith("-"));
                string[] extraParameters = extraArgs.Where(t => t.StartsWith("-")).ToArray();

                if (processingToolsCount > 1)
                    throw new ArgumentException("More than one file processing tool is specified.", nameof(args));

                if (processingToolsCount == 0 && _config.IsPdfToolRequired)
                    throw new ArgumentException("No file processing tool specified.", nameof(args));

                if (extraParameters.Length > 0)
                    throw new UnknownParametersException(extraParameters);

                _config.PdfTool = extraArgs.Count > 0
                    ? extraArgs[0]
                    : null;
            }
            catch (Exception ex)
            {
                exception = ex;
                config = _config;

                return false;
            }

            exception = null;
            config = _config;

            return true;
        }

        #region ParameterParsers

        private static TextAlignment ParseTextAlignment(string data)
        {
            return Enum.Parse<TextAlignment>(data, true);
        }

        private static LocationMode ParseLocationMode(string data)
        {
            return Enum.Parse<LocationMode>(data, true);
        }

        private static TabAlignment ParseTabAlignment(string data)
        {
            return Enum.Parse<TabAlignment>(data, true);
        }

        private static HorizontalAlignment ParseHorizontalAlignment(string data)
        {
            return Enum.Parse<HorizontalAlignment>(data, true);
        }

        private static VerticalAlignment ParseVerticalAlignment(string data)
        {
            return Enum.Parse<VerticalAlignment>(data, true);
        }

        public static PageNumberTextPresenter ParsePageNumberTextPresenter(string data)
        {
            return StandardValues.PageNumberTextPresenters.TryGetValue(data.ToUpper(), out PageNumberTextPresenter? presenter)
                ? presenter
                : throw new IncorrectParameterValueException("PageNumberTextPresenter", data);
        }

        private static string ParseFontFamily(string data)
        {
            return StandardValues.FontFamilies.TryGetValue(data.ToUpper(), out string? fontFamily)
                ? fontFamily
                : throw new IncorrectParameterValueException("FontFamily", data);
        }

        private static string ParseEncoding(string data)
        {
            return StandardValues.Encodings.TryGetValue(data.ToUpper(), out string? encoding)
                ? encoding
                : throw new IncorrectParameterValueException("Encoding", data);
        }

        private static int ParseEncryptionAlgorithm(string data)
        {
            return StandardValues.EncryptionAlgorithms.TryGetValue(data.ToUpper(), out int encryptionAlgorithm)
                ? encryptionAlgorithm
                : throw new IncorrectParameterValueException("EncryptionAlgorithm", data);
        }

        private static long ParseSplittingPartSize(string data)
        {
            if (long.TryParse(data, out long value))
                return value;

            IEnumerable<long> values = data
                .Split('*')
                .Select(t => long.Parse(t));

            return values.Aggregate(1L, (x, y) => x * y);
        }

        private static int ParseTime(string data)
        {
            const string parameterName = "Time";

            if (string.IsNullOrEmpty(data) || data.Length == 0)
                throw new IncorrectParameterValueException(parameterName, data);

            if (int.TryParse(data, out int value))
                return value;

            if (!TimeSpan.TryParse(data, out TimeSpan time))
                throw new IncorrectParameterValueException(parameterName, data);

            double totalMilliseconds = time.TotalMilliseconds;

            if (totalMilliseconds > int.MaxValue)
            {
                string message = $"The '{parameterName}' parameter should represent a time " +
                    $"whose total number of milliseconds does not exceed {int.MaxValue}.";

                throw new OverflowException(message);
            }

            return (int)totalMilliseconds;
        }

        private static PageSize ParsePageSize(string data)
        {
            if (StandardValues.PageSizes.TryGetValue(data.ToUpper(), out PageSize? pageSize))
                return pageSize;

            string[] parts = data.Split('x');

            if (parts.Length != 2)
                throw new IncorrectParameterValueException("PageSize", data);

            int width = int.Parse(parts[0]);
            int height = int.Parse(parts[1]);

            return new PageSize(width, height);
        }

        private static Color ParseColor(string data)
        {
            if (StandardValues.Colors.TryGetValue(data.ToUpper(), out Color? color))
                return color;

            string[] parts = data.Split('-');

            if (parts.Length != 3)
                throw new IncorrectParameterValueException("Color", data);

            int r = int.Parse(parts[0]);
            int g = int.Parse(parts[1]);
            int b = int.Parse(parts[2]);

            return new DeviceRgb(r, g, b);
        }

        #endregion

        public bool Equals(ArgsParser? other)
        {
            return other is not null &&
                   EqualityComparer<YpdfConfig>.Default.Equals(_config, other._config) &&
                   EqualityComparer<OptionSet>.Default.Equals(_options, other._options);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ArgsParser);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_config, _options);
        }
    }
}
