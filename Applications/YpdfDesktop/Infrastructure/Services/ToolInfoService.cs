using YpdfDesktop.Models;
using YpdfDesktop.Models.Localization;

namespace YpdfDesktop.Infrastructure.Services
{
    internal static class ToolInfoService
    {
        public static string? GetIconName(ToolType toolType)
        {
            return toolType switch
            {
                ToolType.Split => "fa-scissors",
                ToolType.Merge => "fa-copy",
                ToolType.Compress => "fa-compress",
                ToolType.HandlePages => "fa-table",
                ToolType.CropPages => "fa-crop",
                ToolType.DividePages => "fa-columns",
                ToolType.AddPageNumbers => "fa-sort-numeric-down",
                ToolType.AddWatermark => "fa-paint-brush",
                ToolType.RemoveWatermark => "fa-eraser",
                ToolType.ImageToPdf => "fa-file-pdf",
                ToolType.TextToPdf => "fa-file-pdf",
                ToolType.ExtractImages => "fa-file-image",
                ToolType.ExtractText => "fa-file-alt",
                ToolType.SetPassword => "fa-lock",
                ToolType.RemovePassword => "fa-unlock",

                _ => null
            };
        }

        public static string? GetToolName(ToolType toolType, ILocale locale)
        {
            return toolType switch
            {
                ToolType.Split => locale.Split,
                ToolType.Merge => locale.Merge,
                ToolType.Compress => locale.Compress,
                ToolType.HandlePages => locale.HandlePages,
                ToolType.CropPages => locale.CropPages,
                ToolType.DividePages => locale.DividePages,
                ToolType.AddPageNumbers => locale.AddPageNumbers,
                ToolType.AddWatermark => locale.AddWatermark,
                ToolType.RemoveWatermark => locale.RemoveWatermark,
                ToolType.ImageToPdf => locale.ImageToPdf,
                ToolType.TextToPdf => locale.TextToPdf,
                ToolType.ExtractImages => locale.ExtractImages,
                ToolType.ExtractText => locale.ExtractText,
                ToolType.SetPassword => locale.SetPassword,
                ToolType.RemovePassword => locale.RemovePassword,

                _ => null
            };
        }
    }
}
