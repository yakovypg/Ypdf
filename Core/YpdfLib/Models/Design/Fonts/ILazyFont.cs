namespace YpdfLib.Models.Design.Fonts
{
    public interface ILazyFont : IDeepCloneable<ILazyFont>
    {
        string Name { get; }

        Font GetFont();
    }
}
