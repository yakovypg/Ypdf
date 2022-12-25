namespace YpdfLib.Models
{
    public interface IShallowCloneable<T>
    {
        T ShallowCopy();
    }
}
