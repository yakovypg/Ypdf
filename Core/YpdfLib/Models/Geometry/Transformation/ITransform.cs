namespace YpdfLib.Models.Geometry.Transformation
{
    public interface ITransform<T>
    {
        T Transform(T obj);
    }
}
