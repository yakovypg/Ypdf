namespace YpdfLib.Models.Enumeration
{
    public class Range : IRange
    {
        public int Start { get; }
        public int End { get; }

        public int Length => End - Start + 1;
        public int[] Items => Enumerable.Range(Start, Length).ToArray();

        public Range() : this(0, 0)
        {
        }

        public Range(int start) : this(start, start)
        {
        }

        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool IsInRange(int value)
        {
            return value >= Start && value <= End;
        }
    }
}
