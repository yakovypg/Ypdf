using iText.Kernel.Geom;
using YpdfLib.Models.Enumeration;
using YpdfLib.Models.Geometry;
using YpdfLib.Models.Parsing;

namespace YpdfLib.Models.Paging
{
    public class PageCropping : IPageCropping, IDeepCloneable<PageCropping>, IEquatable<PageCropping?>
    {
        private const string PARSE_ERROR_MESSAGE = "Incorrect page cropping foramt. Format: Pages:LowerLeftPoint,UpperRightPoint.";

        public int PageNumber { get; }
        public FloatPoint LowerLeft { get; }
        public FloatPoint UpperRight { get; }

        public Rectangle Box
        {
            get
            {
                var lowerLeft = new Point(LowerLeft.X, LowerLeft.Y);
                var upperRight = new Point(UpperRight.X + LowerLeft.X, UpperRight.Y + LowerLeft.Y);
                var pointsList = new List<Point>() { lowerLeft, upperRight };

                return Rectangle.CalculateBBox(pointsList);
            }
        }

        public PageCropping(int pageNumber, FloatPoint lowerLeft, FloatPoint upperRight)
        {
            PageNumber = pageNumber;
            LowerLeft = lowerLeft;
            UpperRight = upperRight;
        }

        public PageCropping Copy()
        {
            return new PageCropping(PageNumber, LowerLeft, UpperRight);
        }

        IPageCropping IDeepCloneable<IPageCropping>.Copy()
        {
            return Copy();
        }

        public bool Equals(PageCropping? other)
        {
            return other is not null &&
                   PageNumber == other.PageNumber &&
                   LowerLeft.Equals(other.LowerLeft) &&
                   UpperRight.Equals(other.UpperRight);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageCropping);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PageNumber, LowerLeft, UpperRight);
        }

        public static PageCropping Parse(string data)
        {
            string[] parts = data.Split(':');

            if (parts.Length != 2)
                throw new ArgumentException(PARSE_ERROR_MESSAGE, nameof(data));

            string[] pointsData = parts[1].Split(',');

            if (pointsData.Length != 2)
                throw new ArgumentException(PARSE_ERROR_MESSAGE, nameof(data));

            int pageNumber = int.Parse(parts[0]);

            FloatPoint lowerLeftPoint = FloatPoint.Parse(pointsData[0]);
            FloatPoint upperRightPoint = FloatPoint.Parse(pointsData[1]);

            return new PageCropping(pageNumber, lowerLeftPoint, upperRightPoint);
        }

        public static PageCropping[] ParseFromRange(string data)
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

        public static PageCropping[] ParseMany(string data)
        {
            var allShifts = AbstractParser.ParseManyToOne(data, ',', ':', ParseFromRange);
            var result = new List<PageCropping>();

            foreach (PageCropping[] shifts in allShifts)
                result.AddRange(shifts);

            return result.ToArray();
        }
    }
}
