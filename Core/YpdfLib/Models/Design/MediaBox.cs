using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace YpdfLib.Models.Design
{
    public class MediaBox : IMediaBox, IEquatable<MediaBox?>
    {
        private readonly PdfPage _page;
        private readonly PdfArray _parameters;

        public float InitialStartX { get; }
        public float InitialStartY { get; }
        public float InitialWidth { get; }
        public float InitialHeight { get; }

        public float StartX
        {
            get => _parameters.GetAsNumber(0).FloatValue();
            set => _parameters.Set(0, new PdfNumber(value));
        }

        public float StartY
        {
            get => _parameters.GetAsNumber(1).FloatValue();
            set => _parameters.Set(1, new PdfNumber(value));
        }

        public float Width
        {
            get => _parameters.GetAsNumber(2).FloatValue();
            set => _parameters.Set(2, new PdfNumber(value));
        }

        public float Height
        {
            get => _parameters.GetAsNumber(3).FloatValue();
            set => _parameters.Set(3, new PdfNumber(value));
        }

        public MediaBox(PdfPage page)
        {
            _page = page;
            _parameters = page.GetPdfObject().GetAsArray(PdfName.MediaBox);

            InitialStartX = StartX;
            InitialStartY = StartY;
            InitialWidth = Width;
            InitialHeight = Height;
        }

        public void DrawRectangle(float x, float y, float width, float height)
        {
            DrawRectangle(x, y, width, height, ColorConstants.WHITE);
        }

        public void DrawRectangle(float x, float y, float width, float height, Color fillColor)
        {
            var overCanvas = new PdfCanvas(_page);

            overCanvas.SaveState();
            overCanvas.SetFillColor(fillColor);
            overCanvas.Rectangle(x, y, width, height);
            overCanvas.Fill();
            overCanvas.RestoreState();
        }

        public bool Equals(MediaBox? other)
        {
            return other is not null &&
                   EqualityComparer<PdfPage>.Default.Equals(_page, other._page) &&
                   EqualityComparer<PdfArray>.Default.Equals(_parameters, other._parameters) &&
                   InitialStartX == other.InitialStartX &&
                   InitialStartY == other.InitialStartY &&
                   InitialWidth == other.InitialWidth &&
                   InitialHeight == other.InitialHeight &&
                   StartX == other.StartX &&
                   StartY == other.StartY &&
                   Width == other.Width &&
                   Height == other.Height;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MediaBox);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(_page);
            hash.Add(_parameters);
            hash.Add(InitialStartX);
            hash.Add(InitialStartY);
            hash.Add(InitialWidth);
            hash.Add(InitialHeight);
            hash.Add(StartX);
            hash.Add(StartY);
            hash.Add(Width);
            hash.Add(Height);

            return hash.ToHashCode();
        }
    }
}
