using System.IO;

namespace YpdfDesktop.Models.IO
{
    public interface IInputFile
    {
        FileInfo File { get; }
        InputFile Self { get; }
    }
}