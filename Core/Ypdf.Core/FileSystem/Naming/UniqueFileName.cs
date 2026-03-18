using System;
using System.IO;

namespace Ypdf.Core.FileSystem.Naming;

public class UniqueFileName : UniqueName
{
    public UniqueFileName(string extension, string workingDirectory)
        : base(t => File.Exists(Path.Combine(workingDirectory, t)), $".{extension}")
    {
        if (string.IsNullOrWhiteSpace(extension))
        {
            throw new ArgumentException(
                $"{nameof(extension)} cannot be an empty string.",
                nameof(extension));
        }

        if (workingDirectory is null)
            throw new ArgumentNullException(nameof(workingDirectory));

        Extension = extension;
        WorkingDirectory = workingDirectory;
    }

    public string Extension { get; }
    public string WorkingDirectory { get; }
}
