using iText.Kernel.Geom;
using YpdfLib.Models.Design;

namespace YpdfLib.Models.Paging
{
    public interface IPageMarginSuggester : IDeepCloneable<IPageMarginSuggester>
    {
        PageContentType PageContentType { get; }
        PageSize? PageSize { get; }

        IMargin Suggest();
    }
}