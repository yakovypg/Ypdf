using iText.Kernel.Geom;
using YpdfLib.Extensions;
using YpdfLib.Models.Design;

namespace YpdfLib.Models.Paging
{
    public class PageMarginSuggester : IPageMarginSuggester, IDeepCloneable<PageMarginSuggester>, IEquatable<PageMarginSuggester?>
    {
        public static IMargin DefaultPageMargin => new Margin(0f);
        public static IMargin DefaultPageNumberMargin => new Margin(2f);
        public static IMargin DefaultImagePageMargin => new Margin(30f);
        public static IMargin DefaultTextPageMargin => new Margin(75.876f);
        public static IMargin DefaultMylticomponentPageMargin => new Margin(30f);

        public PageContentType PageContentType { get; }
        public PageSize? PageSize { get; }

        public PageMarginSuggester(PageContentType pageContentType = PageContentType.UNKNOWN, PageSize? pageSize = null)
        {
            PageContentType = pageContentType;
            PageSize = pageSize;
        }

        public IMargin Suggest()
        {
            IMargin defaultMargin = PageContentType switch
            {
                PageContentType.EMPTY => DefaultPageMargin,
                PageContentType.TEXT => DefaultTextPageMargin,
                PageContentType.IMAGE => DefaultImagePageMargin,
                PageContentType.MULTIPLY => DefaultMylticomponentPageMargin,
                PageContentType.NUMBER => DefaultPageNumberMargin,
                PageContentType.UNKNOWN => DefaultPageMargin,

                _ => DefaultPageMargin
            };

            if (PageSize is null)
                return defaultMargin;

            PageSize defaultPageSize = PageSize.DEFAULT;

            if (defaultPageSize.EqualsWithEpsilon(PageSize))
                return defaultMargin;

            float width = PageSize.GetWidth();
            float height = PageSize.GetHeight();

            float defaultWidth = defaultPageSize.GetWidth();
            float defaultHeight = defaultPageSize.GetHeight();

            float widthFactor = width / defaultWidth;
            float heightFactor = height / defaultHeight;

            float left = defaultMargin.Left * widthFactor;
            float right = defaultMargin.Right * widthFactor;
            float top = defaultMargin.Top * heightFactor;
            float bottom = defaultMargin.Bottom * heightFactor;

            return new Margin(left, top, right, bottom);
        }

        public PageMarginSuggester Copy()
        {
            return new PageMarginSuggester(PageContentType, PageSize?.DeepClone());
        }

        IPageMarginSuggester IDeepCloneable<IPageMarginSuggester>.Copy()
        {
            return Copy();
        }

        public bool Equals(PageMarginSuggester? other)
        {
            return other is not null &&
                   PageContentType == other.PageContentType &&
                   EqualityComparer<PageSize?>.Default.Equals(PageSize, other.PageSize);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageMarginSuggester);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PageContentType, PageSize);
        }

        public static IMargin Suggest(PageContentType pageContentType = PageContentType.UNKNOWN, PageSize? pageSize = null)
        {
            var suggester = new PageMarginSuggester(pageContentType, pageSize);
            return suggester.Suggest();
        }
    }
}
