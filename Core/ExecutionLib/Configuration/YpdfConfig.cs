using YpdfLib.Models.Design;
using YpdfLib.Models.Design.Fonts;
using YpdfLib.Models.Enumeration;
using YpdfLib.Models.Geometry;
using YpdfLib.Models.Imaging;
using YpdfLib.Models.Paging;
using YpdfLib.Models.Security;

namespace ExecutionLib.Configuration
{
    public class YpdfConfig
    {
        public string? PdfTool { get; set; }

        public bool ShowHelp { get; set; }
        public bool ShowVersion { get; set; }
        public bool ShowGuide { get; set; }

        public bool ShowGlobalConfig { get; set; }
        public bool SaveGlobalConfig { get; set; }
        public bool ResetGlobalConfig { get; set; }

        public int? RotationAngle { get; set; }
        public long? SplittingPartSize { get; set; }

        public IMargin? Margin { get; set; }
        public IPageOrder? PageOrder { get; set; }

        public ICollection<IPageRange> Pages { get; } = new List<IPageRange>();
        public ICollection<IPageRotation> PageRotations { get; } = new List<IPageRotation>();
        public ICollection<IPageCropping> PageCroppings { get; } = new List<IPageCropping>();
        public ICollection<IPageDivision> PageDivisions { get; } = new List<IPageDivision>();
        public ICollection<IPageContentShift> ContentShifts { get; set; } = new List<IPageContentShift>();

        public IPathsConfig PathsConfig { get; } = new PathsConfig();
        public IYpdfGlobalConfig GlobalConfig { get; } = new YpdfGlobalConfig();

        public IFontInfo FontInfo { get; } = new FontInfo();
        public IImageCompression ImageCompression { get; } = new ImageCompression();
        public IPageMovementInfo PageMovement { get; set; } = new PageMovementInfo();
        public IWatermarkAnnotation Watermark { get; } = new WatermarkAnnotation();
        public IPdfPassword PdfPassword { get; } = new PdfPassword();

        public IPageChange PageChange { get; } = new PageChange();
        public IPageNumberStyle PageNumberStyle { get; } = new PageNumberStyle();
        public IImagePageParameters ImagePageParameters { get; } = new ImagePageParameters();
        public ITextPageParameters TextPageParameters { get; } = new TextPageParameters();

        public int[] PageNumbers => PageRange.GetAllItems(Pages);
        public bool IsPdfToolRequired => !ShowHelp && !ShowVersion && !ShowGuide;

        #region ConfigurationMethods

        public IImagePageParameters GetConfiguredImagePageParameters()
        {
            IImagePageParameters parameters = ImagePageParameters.Copy();

            parameters.Size = PageChange.PageSize;
            parameters.RotationAngle = Angle.DegreesToRadians(RotationAngle ?? 0);
            parameters.Margin = Margin?.Copy() ?? PageMarginSuggester.Suggest(PageContentType.IMAGE, parameters.Size);

            return parameters;
        }

        public ITextPageParameters GetConfiguredTextPageParameters()
        {
            ITextPageParameters parameters = TextPageParameters.Copy();

            parameters.FontInfo = FontInfo.Copy();
            parameters.Margin = Margin?.Copy() ?? PageMarginSuggester.Suggest(PageContentType.TEXT, parameters.PageSize);

            return parameters;
        }

        public IPageNumberStyle GetConfiguredPageNumberStyle()
        {
            IPageNumberStyle style = PageNumberStyle.Copy();

            style.FontInfo = FontInfo.Copy();
            style.Margin = Margin?.Copy() ?? PageMarginSuggester.Suggest(PageContentType.NUMBER);

            return style;
        }

        public IWatermarkAnnotation GetConfiguredWatermarkAnnotation()
        {
            IWatermarkAnnotation watermark = Watermark.Copy();

            watermark.FontInfo = FontInfo.Copy();
            watermark.SetRotationAngleInDegrees(RotationAngle ?? 0);

            return watermark;
        }

        public IIndelibleWatermark GetConfiguredIndelibleWatermark()
        {
            return GetConfiguredWatermarkAnnotation().ToIndelibleWatermark();
        }

        #endregion
    }
}
