using iText.Kernel.Pdf;

namespace YpdfLib.Models.Paging
{
    public class PageMovement : IPageMovement, IEquatable<PageMovement?>
    {
        public PdfPage Page { get; }
        public int Position { get; }

        public PageMovement(PdfPage page, int position)
        {
            Page = page;
            Position = position;
        }

        public bool Equals(PageMovement? other)
        {
            return other is not null &&
                   EqualityComparer<PdfPage>.Default.Equals(Page, other.Page) &&
                   Position == other.Position;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageMovement);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Page, Position);
        }
    }
}
