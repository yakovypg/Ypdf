namespace YpdfLib.Models.Enumeration
{
    public interface IRange
    {
        int Start { get; }
        int End { get; }

        int Length { get; }
        int[] Items { get; }

        bool IsInRange(int value);
    }
}
