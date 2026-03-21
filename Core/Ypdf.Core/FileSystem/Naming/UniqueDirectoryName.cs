using System;
using System.IO;

namespace Ypdf.Core.FileSystem.Naming;

public class UniqueDirectoryName : UniqueName
{
    public UniqueDirectoryName(string workingDirectory)
        : base(t => Directory.Exists(Path.Combine(workingDirectory, t)))
    {
        ExtendedArgumentNullException.ThrowIfNull(workingDirectory, nameof(workingDirectory));
        WorkingDirectory = workingDirectory;
    }

    public string WorkingDirectory { get; }
}
