namespace YpdfLib.Models.Paging
{
    public interface IPageContentShift : IDeepCloneable<IPageContentShift>
    {
        int PageNumber { get; set; }
        float Horizontal { get; set; }
        float Vertical { get; set; }
    }
}
