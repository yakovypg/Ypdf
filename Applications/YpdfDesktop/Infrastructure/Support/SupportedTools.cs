using System.Collections.ObjectModel;
using YpdfDesktop.Models;

namespace YpdfDesktop.Infrastructure.Support
{
    internal static class SupportedTools
    {
        public static ObservableCollection<Tool> Get()
        {
            return new ObservableCollection<Tool>()
            {
                new Tool("split", "fa-scissors"),
                new Tool("merge", "fa-copy"),
                new Tool("compress", "fa-compress"),
                new Tool("handle pages", "fa-table"),
                new Tool("crop pages", "fa-crop"),
                new Tool("divide pages", "fa-columns"),
                new Tool("add page nums", "fa-sort-numeric-down"),
                new Tool("add watermark", "fa-paint-brush"),
                new Tool("rm watermark", "fa-eraser"),
                new Tool("image2pdf", "fa-file-pdf"),
                new Tool("text2pdf", "fa-file-pdf"),
                new Tool("extract images", "fa-file-image"),
                new Tool("extract text", "fa-file-alt"),
                new Tool("set password", "fa-lock"),
                new Tool("rm password", "fa-unlock")
            };
        }
    }
}
