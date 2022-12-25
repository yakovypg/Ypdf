namespace YpdfLib.Models.Paging
{
    public interface IPageIncrease : IDeepCloneable<IPageIncrease>
    {
        float Left { get; set; }
        float Top { get; set; }
        float Right { get; set; }
        float Bottom { get; set; }
    }
}
