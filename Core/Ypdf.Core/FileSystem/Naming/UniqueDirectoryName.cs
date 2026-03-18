using System;
using System.IO;

namespace Ypdf.Core.FileSystem.Naming;

public class UniqueDirectoryName : UniqueName
{
    public UniqueDirectoryName(string workingDirectory)
        : base(t => Directory.Exists(Path.Combine(workingDirectory, t)))
    {
        WorkingDirectory = workingDirectory
            ?? throw new ArgumentNullException(nameof(workingDirectory));
    }

    public string WorkingDirectory { get; }
}
