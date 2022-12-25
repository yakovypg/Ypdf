namespace YpdfLib.Models.Design
{
    public interface IMargin : IDeepCloneable<IMargin>
    {
        float Left { get; }
        float Top { get; }
        float Right { get; }
        float Bottom { get; }

        float HorizontalSum { get; }
        float VerticalSum { get; }

        bool IsZero { get; }
    }
}
