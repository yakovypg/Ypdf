namespace YpdfLib.Models.Design
{
    public interface IMargin
    {
        float Top { get; }
        float Right { get; }
        float Bottom { get; }
        float Left { get; }

        float HorizontalSum { get; }
        float VerticalSum { get; }
    }
}
