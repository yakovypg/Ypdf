namespace YpdfLib.Models.Paging
{
    public interface IPageDivision : IDeepCloneable<IPageDivision>
    {
        int PageNumber { get; }
        float CenterOffset { get; }
        PageDivisionOrientation Orientation { get; }
    }
}
