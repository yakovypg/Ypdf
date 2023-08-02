namespace YpdfDesktop.Models.Paging
{
    public interface IPageInfo
    {
        int PageNumber { get; }
        int Position { get; set; }
        int RotationAngle { get; set; }
    }
}
