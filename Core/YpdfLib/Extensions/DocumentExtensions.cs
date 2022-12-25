using iText.Layout;
using YpdfLib.Models.Design;

namespace YpdfLib.Extensions
{
    internal static class DocumentExtensions
    {
        public static IMargin GetMargin(this Document doc)
        {
            var margin = new Margin(
                doc.GetLeftMargin(),
                doc.GetTopMargin(),
                doc.GetRightMargin(),
                doc.GetBottomMargin()
            );

            return margin;
        }
    }
}
