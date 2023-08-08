namespace YpdfDesktop.Models.Paging
{
    public class PageInfo : IPageInfo
    {
        public int PageNumber { get; }
        public float Width { get; set; }
        public float Height { get; set; }

        public PageInfo(int pageNumber, float width, float height)
        {
            PageNumber = pageNumber;
            Width = width;
            Height = height;
        }
    }
}
