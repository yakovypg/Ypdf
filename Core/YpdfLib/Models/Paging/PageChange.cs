using iText.Kernel.Colors;
using iText.Kernel.Geom;

namespace YpdfLib.Models.Paging
{
    public class PageChange : IPageChange, IDeepCloneable<PageChange>, IEquatable<PageChange?>
    {
        public int? NewWidth { get; set; }
        public int? NewHeight { get; set; }
        public IPageIncrease? Increase { get; set; }
        public Color FillColor { get; set; }

        public PageSize? PageSize
        {
            get
            {
                PageSize? pageSize = NewWidth is not null && NewHeight is not null
                    ? new PageSize(NewWidth.Value, NewHeight.Value)
                    : null;

                return pageSize;
            }
            set
            {
                if (value is null)
                {
                    NewWidth = NewHeight = null;
                    return;
                }

                NewWidth = (int)value.GetWidth();
                NewHeight = (int)value.GetHeight();
            }
        }

        public PageChange(int? newWidth = null, int? newHeight = null, IPageIncrease? increase = null) :
            this(newWidth, newHeight, increase, ColorConstants.WHITE)
        {
        }

        public PageChange(int? newWidth, int? newHeight, IPageIncrease? increase, Color fillColor)
        {
            NewWidth = newWidth;
            NewHeight = newHeight;
            Increase = increase;
            FillColor = fillColor;
        }

        public PageChange Copy()
        {
            return new PageChange(NewWidth, NewHeight, Increase?.Copy(), FillColor);
        }

        IPageChange IDeepCloneable<IPageChange>.Copy()
        {
            return Copy();
        }

        public bool Equals(PageChange? other)
        {
            return other is not null &&
                   NewWidth == other.NewWidth &&
                   NewHeight == other.NewHeight &&
                   EqualityComparer<IPageIncrease?>.Default.Equals(Increase, other.Increase) &&
                   EqualityComparer<Color>.Default.Equals(FillColor, other.FillColor);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageChange);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NewWidth, NewHeight, Increase, FillColor);
        }
    }
}
