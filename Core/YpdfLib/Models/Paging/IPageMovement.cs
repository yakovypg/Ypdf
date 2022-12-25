using iText.Kernel.Pdf;

namespace YpdfLib.Models.Paging
{
    public interface IPageMovement
    {
        PdfPage Page { get; }
        int Position { get; }
    }
}
