namespace YpdfDesktop.Models.Paging
{
    public interface IPageHandlingInfo
    {
        int PageNumber { get; }
        int Position { get; set; }
        int RotationAngle { get; set; }
    }
}
