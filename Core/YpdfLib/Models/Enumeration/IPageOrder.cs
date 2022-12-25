namespace YpdfLib.Models.Enumeration
{
    public interface IPageOrder : IDeepCloneable<IPageOrder>
    {
        int[] Pages { get; }
    }
}
