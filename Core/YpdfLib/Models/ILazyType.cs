namespace YpdfLib.Models
{
    public interface ILazyType<T>
    {
        T Create();
    }
}
