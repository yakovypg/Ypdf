namespace YpdfLib.Models.Design.Fonts
{
    public interface ILazyFont : ILazyType<Font>, IDeepCloneable<ILazyFont>
    {
        string Name { get; }
    }
}
