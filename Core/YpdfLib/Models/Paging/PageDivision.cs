using YpdfLib.Models.Enumeration;
using YpdfLib.Models.Parsing;

namespace YpdfLib.Models.Paging
{
    public class PageDivision : IPageDivision, IDeepCloneable<PageDivision>, IEquatable<PageDivision?>
    {
        private const string PARSE_ERROR_MESSAGE = "Incorrect page cropping foramt. Format: Pages:Orientation,CenterOffset.";

        public int PageNumber { get; }
        public float CenterOffset { get; }
        public PageDivisionOrientation Orientation { get; }

        public PageDivision(int pageNumber, PageDivisionOrientation orientation, float centerOffset = 0)
        {
            PageNumber = pageNumber;
            Orientation = orientation;
            CenterOffset = centerOffset;
        }

        public PageDivision Copy()
        {
            return new PageDivision(PageNumber, Orientation, CenterOffset);
        }

        IPageDivision IDeepCloneable<IPageDivision>.Copy()
        {
            return Copy();
        }

        public bool Equals(PageDivision? other)
        {
            return other is not null &&
                   PageNumber == other.PageNumber &&
                   CenterOffset == other.CenterOffset &&
                   Orientation == other.Orientation;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageDivision);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PageNumber, CenterOffset, Orientation);
        }

        public static PageDivision Parse(string data)
        {
            string[] parts = data.Split(':');

            if (parts.Length != 2)
                throw new ArgumentException(PARSE_ERROR_MESSAGE, nameof(data));

            string[] additionalData = parts[1].Split(',');

            if (additionalData.Length < 1 || additionalData.Length > 2)
                throw new ArgumentException(PARSE_ERROR_MESSAGE, nameof(data));

            int pageNumber = int.Parse(parts[0]);
            var orientation = Enum.Parse<PageDivisionOrientation>(additionalData[0], true);

            float centerOffset = additionalData.Length == 2
                ? float.Parse(additionalData[1])
                : 0;

            return new PageDivision(pageNumber, orientation, centerOffset);
        }

        public static PageDivision[] ParseFromRange(string data)
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

        public static PageDivision[] ParseMany(string data)
        {
            var allShifts = AbstractParser.ParseManyToOne(data, ',', ':', ParseFromRange);
            var result = new List<PageDivision>();

            foreach (PageDivision[] shifts in allShifts)
                result.AddRange(shifts);

            return result.ToArray();
        }
    }
}
