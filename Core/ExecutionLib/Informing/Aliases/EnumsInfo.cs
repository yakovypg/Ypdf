using iText.Layout.Properties;
using System.Collections.Generic;
using YpdfLib.Models.Design;

namespace ExecutionLib.Informing.Aliases
{
    public static class EnumsInfo
    {
        public static string LocationModeValues => string.Join('/', Enum.GetValues<LocationMode>().Select(t => t.ToString().ToLower()));
        public static string TextAlignmentValues => string.Join('/', Enum.GetValues<TextAlignment>().Select(t => t.ToString().ToLower()));
        public static string TabAlignmentValues => string.Join('/', Enum.GetValues<TabAlignment>().Select(t => t.ToString().ToLower()));
        public static string HorizontalAlignmentValues => string.Join('/', Enum.GetValues<HorizontalAlignment>().Select(t => t.ToString().ToLower()));
        public static string VerticalAlignmentValues => string.Join('/', Enum.GetValues<VerticalAlignment>().Select(t => t.ToString().ToLower()));
        public static string BorderTypeValues => string.Join('/', Enum.GetValues<BorderType>().Select(t => t.ToString().ToLower()));

        public static IReadOnlyList<LocationMode> LocationModes => Enum.GetValues<LocationMode>().ToList();
        public static IReadOnlyList<TextAlignment> TextAlignments => Enum.GetValues<TextAlignment>().ToList();
        public static IReadOnlyList<TabAlignment> TabAlignments => Enum.GetValues<TabAlignment>().ToList();
        public static IReadOnlyList<HorizontalAlignment> HorizontalAlignments => Enum.GetValues<HorizontalAlignment>().ToList();
        public static IReadOnlyList<VerticalAlignment> VerticalAlignments => Enum.GetValues<VerticalAlignment>().ToList();
        public static IReadOnlyList<BorderType> BorderTypes => Enum.GetValues<BorderType>().ToList();
    }
}
