namespace YpdfLib.Models.Enumeration
{
    public interface IPageRange : IDeepCloneable<IPageRange>
    {
        int Start { get; }
        int End { get; }

        int Length { get; }
        int[] Items { get; }

        bool IsInRange(int value);
    }
}
