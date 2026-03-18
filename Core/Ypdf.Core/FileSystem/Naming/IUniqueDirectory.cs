using System.IO;

namespace Ypdf.Core.FileSystem.Naming;

public interface IUniqueDirectory
{
    DirectoryInfo Create();
}
