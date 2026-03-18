using System.IO;

namespace Ypdf.Core.FileSystem.Naming;

public interface IUniqueFile
{
    FileInfo Create();
}
