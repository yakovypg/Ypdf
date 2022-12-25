using iText.Layout.Properties;
using YpdfLib.Models.Design;

namespace ypdf.Informing
{
    internal static class EnumsInfo
    {
        public static string LocationModeValues => string.Join('/', Enum.GetValues<LocationMode>().Select(t => t.ToString().ToLower()));
        public static string TextAlignmentValues => string.Join('/', Enum.GetValues<TextAlignment>().Select(t => t.ToString().ToLower()));
        public static string TabAlignmentValues => string.Join('/', Enum.GetValues<TabAlignment>().Select(t => t.ToString().ToLower()));
        public static string HorizontalAlignmentValues => string.Join('/', Enum.GetValues<HorizontalAlignment>().Select(t => t.ToString().ToLower()));
        public static string VerticalAlignmentValues => string.Join('/', Enum.GetValues<VerticalAlignment>().Select(t => t.ToString().ToLower()));
    }
}
