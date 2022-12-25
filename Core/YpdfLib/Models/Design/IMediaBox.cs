using iText.Kernel.Colors;

namespace YpdfLib.Models.Design
{
    public interface IMediaBox
    {
        float InitialStartX { get; }
        float InitialStartY { get; }
        float InitialWidth { get; }
        float InitialHeight { get; }

        float StartX { get; set; }
        float StartY { get; set; }
        float Width { get; set; }
        float Height { get; set; }

        void DrawRectangle(float x, float y, float width, float height);
        void DrawRectangle(float x, float y, float width, float height, Color fillColor);
    }
}
