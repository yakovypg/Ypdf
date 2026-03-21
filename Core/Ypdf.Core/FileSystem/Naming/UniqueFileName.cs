using System;
using System.IO;

namespace Ypdf.Core.FileSystem.Naming;

public class UniqueFileName : UniqueName
{
    public UniqueFileName(string extension, string workingDirectory)
        : base(t => File.Exists(Path.Combine(workingDirectory, t)), $".{extension}")
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(extension, nameof(extension));
        ExtendedArgumentNullException.ThrowIfNull(workingDirectory, nameof(workingDirectory));

        Extension = extension;
        WorkingDirectory = workingDirectory;
    }

    public string Extension { get; }
    public string WorkingDirectory { get; }
}
