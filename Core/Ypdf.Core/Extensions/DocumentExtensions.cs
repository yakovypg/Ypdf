using iText.Layout;
using Ypdf.Core.Design;

namespace Ypdf.Core.Extensions;

public static class DocumentExtensions
{
    public static Margin GetMargin(this Document document)
    {
        ExtendedArgumentNullException.ThrowIfNull(document, nameof(document));

        return new Margin(
            document.GetLeftMargin(),
            document.GetTopMargin(),
            document.GetRightMargin(),
            document.GetBottomMargin());
    }
}
