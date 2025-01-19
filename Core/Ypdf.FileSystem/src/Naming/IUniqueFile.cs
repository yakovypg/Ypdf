using System.IO;

namespace Ypdf.FileSystem.Naming;

public interface IUniqueFile
{
    FileInfo Create();
}
