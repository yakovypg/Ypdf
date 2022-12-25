using YpdfLib.Models.Parsing;

namespace YpdfLib.Models.Enumeration
{
    public class PageOrder : IPageOrder, IDeepCloneable<PageOrder>, IEquatable<PageOrder?>
    {
        private readonly int[] _pages;
        public int[] Pages => _pages.Clone() as int[] ?? Array.Empty<int>();

        public PageOrder(int[] pages)
        {
            _pages = pages;
        }

        public PageOrder(int[] pages, int length)
        {
            if (pages.Length < length)
            {
                _pages = pages.Take(length).ToArray();
                return;
            }

            var allPages = Enumerable.Range(1, length);
            var remainingPages = allPages.Except(pages);

            _pages = pages.Concat(remainingPages).ToArray();
        }

        public PageOrder Copy()
        {
            return new PageOrder(_pages);
        }

        IPageOrder IDeepCloneable<IPageOrder>.Copy()
        {
            return Copy();
        }

        public bool Equals(PageOrder? other)
        {
            return other is not null &&
                   EqualityComparer<int[]>.Default.Equals(_pages, other._pages);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageOrder);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_pages);
        }

        public static PageOrder Parse(string data)
        {
            IPageRange[] pageRanges = AbstractParser.ParseMany(data, ',', PageRange.Parse);
            int[] pages = PageRange.GetAllItems(pageRanges);

            return new PageOrder(pages);
        }
    }
}
