using iText.Kernel.Geom;

namespace YpdfLib.Extensions
{
    public static class PageSizeExtesions
    {
        public static PageSize DeepClone(this PageSize pageSize)
        {
            return new PageSize(pageSize.Clone());
        }
    }
}
