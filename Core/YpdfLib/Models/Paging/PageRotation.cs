using YpdfLib.Models.Enumeration;
using YpdfLib.Models.Parsing;

namespace YpdfLib.Models.Paging
{
    public class PageRotation : IPageRotation, IDeepCloneable<PageRotation>, IEquatable<PageRotation?>
    {
        private const string PARSE_ERROR_MESSAGE = "Incorrect page rotation foramt. Format: Pages:Angle.";

        public int PageNumber { get; }
        public int Angle { get; }

        public PageRotation(int pageNumber, int angle)
        {
            PageNumber = pageNumber;
            Angle = angle;
        }

        public PageRotation Copy()
        {
            return new PageRotation(PageNumber, Angle);
        }

        IPageRotation IDeepCloneable<IPageRotation>.Copy()
        {
            return Copy();
        }

        public bool Equals(PageRotation? other)
        {
            return other is not null &&
                   PageNumber == other.PageNumber &&
                   Angle == other.Angle;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageRotation);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PageNumber, Angle);
        }

        public static PageRotation Parse(string data)
        {
            string[] parts = data.Split(':');

            if (parts.Length != 2)
                throw new ArgumentException(PARSE_ERROR_MESSAGE, nameof(data));

            int pageNumber = int.Parse(parts[0]);
            int angle = int.Parse(parts[1]);

            return new PageRotation(pageNumber, angle);
        }

        public static PageRotation[] ParseFromRange(string data)
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

        public static PageRotation[] ParseMany(string data)
        {
            var allShifts = AbstractParser.ParseManyToOne(data, ',', ':', ParseFromRange);
            var result = new List<PageRotation>();

            foreach (PageRotation[] shifts in allShifts)
                result.AddRange(shifts);

            return result.ToArray();
        }
    }
}
