namespace YpdfLib.Security
{
    public interface IPasswordGenerator
    {
        bool IsOver { get; }
        string? Next();
    }
}
