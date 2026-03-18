using System;
using System.IO;

namespace Ypdf.Core.FileSystem.Naming;

public class UniqueDirectory : UniqueDirectoryName, IUniqueDirectory
{
    public UniqueDirectory(string workingDirectory)
        : base(workingDirectory) { }

    public static DirectoryInfo Create(string workingDirectory)
    {
        if (workingDirectory is null)
            throw new ArgumentNullException(nameof(workingDirectory));

        var uniqueDirectory = new UniqueDirectory(workingDirectory);
        return uniqueDirectory.Create();
    }

    public override string GetNext()
    {
        string uniqueDirName = base.GetNext();
        return Path.Combine(WorkingDirectory, uniqueDirName);
    }

    public DirectoryInfo Create()
    {
        string path = GetNext();
        return Directory.CreateDirectory(path);
    }
}
