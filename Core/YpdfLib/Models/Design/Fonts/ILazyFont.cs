namespace YpdfLib.Models.Design.Fonts
{
    public interface ILazyFont
    {
        string Name { get; }
        Font GetFont();
    }
}
