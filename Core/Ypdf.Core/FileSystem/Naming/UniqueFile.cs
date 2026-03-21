using System;
using System.IO;

namespace Ypdf.Core.FileSystem.Naming;

public class UniqueFile : UniqueFileName, IUniqueFile
{
    public UniqueFile(string extension, string workingDirectory)
        : base(extension, workingDirectory) { }

    public static FileInfo Create(string extension, string workingDirectory)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(extension, nameof(extension));
        ExtendedArgumentNullException.ThrowIfNull(workingDirectory, nameof(workingDirectory));

        var uniqueFile = new UniqueFile(extension, workingDirectory);
        return uniqueFile.Create();
    }

    public virtual string MakeUnique(string nameWithoutExtension)
    {
        ExtendedArgumentNullException.ThrowIfNull(nameWithoutExtension, nameof(nameWithoutExtension));

        string pathWithoutExtension = Path.Combine(WorkingDirectory, nameWithoutExtension);
        string uniquePath = $"{pathWithoutExtension}.{Extension}";

        int counter = 0;

        while (File.Exists(uniquePath))
        {
            uniquePath = $"{pathWithoutExtension} ({++counter}).{Extension}";
        }

        return uniquePath;
    }

    public override string GetNext()
    {
        string uniqueFileName = base.GetNext();
        return Path.Combine(WorkingDirectory, uniqueFileName);
    }

    public FileInfo Create()
    {
        string path = GetNext();
        File.Create(path).Close();

        return new FileInfo(path);
    }
}
