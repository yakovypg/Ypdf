namespace YpdfLib.Models.Geometry
{
    public interface IFloatRectangle
    {
        FloatPoint TopLeft { get; }
        FloatPoint TopRight { get; }
        FloatPoint BottomRight { get; }
        FloatPoint BottomLeft { get; }
        CoordinatesMirroring CoordinatesMirroring { get; }

        FloatPoint Center { get; }

        float Left { get; }
        float Top { get; }
        float Right { get; }
        float Bottom { get; }

        float Width { get; }
        float Height { get; }
        FloatSize Size { get; }
    }
}
