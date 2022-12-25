using YpdfLib.Models.Enumeration;
using YpdfLib.Models.Parsing;

namespace YpdfLib.Models.Paging
{
    public class PageContentShift : IPageContentShift, IDeepCloneable<PageContentShift>, IEquatable<PageContentShift?>
    {
        private const string PARSE_ERROR_MESSAGE = "Incorrect page content shift foramt. Format: Page:Horizontal,Vertical.";

        public int PageNumber { get; set; }
        public float Horizontal { get; set; }
        public float Vertical { get; set; }

        public PageContentShift(int pageNumber, float horizontal, float vertical)
        {
            PageNumber = pageNumber;
            Horizontal = horizontal;
            Vertical = vertical;
        }

        public PageContentShift Copy()
        {
            return new PageContentShift(PageNumber, Horizontal, Vertical);
        }

        IPageContentShift IDeepCloneable<IPageContentShift>.Copy()
        {
            return Copy();
        }

        public bool Equals(PageContentShift? other)
        {
            return other is not null &&
                   PageNumber == other.PageNumber &&
                   Horizontal == other.Horizontal &&
                   Vertical == other.Vertical;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageContentShift);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PageNumber, Horizontal, Vertical);
        }

        public static PageContentShift Parse(string data)
        {
            string[] parts = data.Split(':');

            if (parts.Length != 2)
                throw new ArgumentException(PARSE_ERROR_MESSAGE, nameof(data));

            string[] rightParts = parts[1].Split(',');

            if (rightParts.Length != 2)
                throw new ArgumentException(PARSE_ERROR_MESSAGE, nameof(data));

            int pageNumber = int.Parse(parts[0]);
            int horizontal = int.Parse(rightParts[0]);
            int vertical = int.Parse(rightParts[1]);

            return new PageContentShift(pageNumber, horizontal, vertical);
        }

        public static PageContentShift[] ParseFromRange(string data)
        {
            string[] parts = data.Split(':');

            if (parts.Length != 2)
                throw new ArgumentException(PARSE_ERROR_MESSAGE, nameof(data));

            int[] pages = PageRange.Parse(parts[0]).Items;

            return pages
                .Select(t => $"{t}:{parts[1]}")
                .Select(t => Parse(t))
                .ToArray();
        }

        public static PageContentShift[] ParseMany(string data)
        {
            var allShifts = AbstractParser.ParseManyToOne(data, ',', ':', ParseFromRange);
            var result = new List<PageContentShift>();

            foreach (PageContentShift[] shifts in allShifts)
                result.AddRange(shifts);

            return result.ToArray();
        }
    }
}
