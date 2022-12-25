namespace YpdfLib.Models.Paging
{
    public interface IPageRotation : IDeepCloneable<IPageRotation>
    {
        int PageNumber { get; }
        int Angle { get; }
    }
}
