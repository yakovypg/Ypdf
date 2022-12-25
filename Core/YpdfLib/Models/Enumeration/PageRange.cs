namespace YpdfLib.Models.Enumeration
{
    public class PageRange : IPageRange, IDeepCloneable<PageRange>, IEquatable<PageRange?>
    {
        public int Start { get; }
        public int End { get; }

        public int Length => Math.Abs(End - Start) + 1;

        public int[] Items
        {
            get
            {
                var items = Enumerable.Range(Math.Min(Start, End), Length);

                return Start <= End
                    ? items.ToArray()
                    : items.Reverse().ToArray();
            }
        }

        public PageRange() : this(0, 0)
        {
        }

        public PageRange(int start) : this(start, start)
        {
        }

        public PageRange(int start, int end)
        {
            if (start <= 0)
                throw new ArgumentException("The start of the range must be greater than zero.", nameof(start));

            Start = start;
            End = end;
        }

        public bool IsInRange(int value)
        {
            return value >= Start && value <= End;
        }

        public PageRange Copy()
        {
            return new PageRange(Start, End);
        }

        IPageRange IDeepCloneable<IPageRange>.Copy()
        {
            return Copy();
        }

        public override string ToString()
        {
            return Start != End
                ? $"{Start}-{End}"
                : $"{Start}";
        }

        public bool Equals(PageRange? other)
        {
            return other is not null &&
                   Start == other.Start &&
                   End == other.End;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageRange);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }

        public static int[] GetAllItems(IEnumerable<IPageRange> ranges)
        {
            var pages = new List<int>();

            foreach (var range in ranges ?? Array.Empty<IPageRange>())
                pages.AddRange(range.Items);

            return pages.ToArray();
        }

        public static PageRange Parse(string data)
        {
            if (!data.Contains('-'))
            {
                int value = int.Parse(data);
                return new PageRange(value);
            }
            else
            {
                string[] parts = data.Split('-');

                if (parts.Length != 2)
                    throw new ArgumentException("Incorrect range format. Format: S-E.", nameof(data));

                int start = int.Parse(parts[0]);
                int end = int.Parse(parts[1]);

                return new PageRange(start, end);
            }
        }
    }
}
