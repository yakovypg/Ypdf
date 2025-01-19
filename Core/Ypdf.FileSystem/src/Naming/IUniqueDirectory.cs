using System.IO;

namespace Ypdf.FileSystem.Naming;

public interface IUniqueDirectory
{
    DirectoryInfo Create();
}
