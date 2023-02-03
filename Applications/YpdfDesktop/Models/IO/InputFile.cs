using System.IO;

namespace YpdfDesktop.Models.IO
{
    public class InputFile : IInputFile
    {
        public FileInfo File { get; }
        public InputFile Self => this;

        public InputFile(string path) : this(new FileInfo(path))
        {
        }

        public InputFile(FileInfo fileInfo)
        {
            File = fileInfo;
        }
    }
}
