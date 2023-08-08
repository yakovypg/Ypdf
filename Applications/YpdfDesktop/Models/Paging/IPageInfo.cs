namespace YpdfDesktop.Models.Paging
{
    public interface IPageInfo
    {
        int PageNumber { get; }
        float Width { get; set; }
        float Height { get; set; }
    }
}
