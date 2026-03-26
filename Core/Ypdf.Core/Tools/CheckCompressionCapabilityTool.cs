using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ypdf.Core.Compression;
using Ypdf.Core.Config;
using Ypdf.Core.FileSystem.Naming;
using Ypdf.Core.Runtime.Logging;

namespace Ypdf.Core.Tools;

public class CheckCompressionCapabilityTool : ICheckingTool
{
    private const int _numberOfImagesToCheckCompressionCapability = 5;

    public CheckCompressionCapabilityTool(
        string? pythonAlias = null,
        string? virtualEnvironmentPath = null,
        IOutputWriter? outputWriter = null)
    {
        PythonAlias = pythonAlias;
        VirtualEnvironmentPath = virtualEnvironmentPath;
        OutputWriter = outputWriter;
    }

    protected string? PythonAlias { get; }
    protected string? VirtualEnvironmentPath { get; }
    protected IOutputWriter? OutputWriter { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        bool checkResult = Execute(inputPath);
        string resultContent = checkResult.ToString();

        if (string.IsNullOrWhiteSpace(outputPath))
            OutputWriter?.WriteLine(resultContent);
        else
            File.WriteAllText(outputPath, resultContent);
    }

    public bool Execute(string inputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        DirectoryInfo uniqueDirectory = UniqueDirectory.Create(CoreDirectories.TempDirectory);
        IList<FileInfo> extractedImages = ExtractImages(inputPath, uniqueDirectory);

        IEnumerable<string> extractedImagePaths = extractedImages
            .Select(t => t.FullName);

        IList<FileInfo> compressedImages = CompressImages(extractedImagePaths, uniqueDirectory);

        Directory.Delete(uniqueDirectory.FullName, true);
        return VerifyCompressionCapability(extractedImages, compressedImages);
    }

    private static bool VerifyCompressionCapability(
        IList<FileInfo> extractedImages,
        IList<FileInfo> compressedImages)
    {
        ExtendedArgumentNullException.ThrowIfNull(extractedImages, nameof(extractedImages));
        ExtendedArgumentNullException.ThrowIfNull(compressedImages, nameof(compressedImages));

        if (compressedImages.Count != extractedImages.Count)
            throw new CompressionException("Compression capability check failed.");

        for (int i = 0; i < extractedImages.Count; ++i)
        {
            if (extractedImages[i].Length < compressedImages[i].Length)
                return false;
        }

        return true;
    }

    private List<FileInfo> ExtractImages(string inputPath, DirectoryInfo uniqueDirectory)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentNullException.ThrowIfNull(uniqueDirectory, nameof(uniqueDirectory));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        var pdfToImageTool = new PdfToImageTool(
            _numberOfImagesToCheckCompressionCapability,
            PythonAlias,
            VirtualEnvironmentPath,
            OutputWriter);

        pdfToImageTool.Execute(inputPath, uniqueDirectory.FullName);

        IEnumerable<FileInfo> extractedImages = uniqueDirectory
            .GetFiles()
            .OrderBy(t => t.Name);

        return [.. extractedImages];
    }

    private List<FileInfo> CompressImages(
        IEnumerable<string> inputPaths,
        DirectoryInfo uniqueDirectory)
    {
        ExtendedArgumentNullException.ThrowIfNull(inputPaths, nameof(inputPaths));
        DefaultExceptions.ThrowIfContainsNotExistingFile(inputPaths, nameof(inputPaths));
        ExtendedArgumentNullException.ThrowIfNull(uniqueDirectory, nameof(uniqueDirectory));

        var compressImageTool = new CompressImageTool(default, PythonAlias, VirtualEnvironmentPath, OutputWriter);
        compressImageTool.Execute(inputPaths, uniqueDirectory.FullName);

        IEnumerable<FileInfo> compressedImages = uniqueDirectory
            .GetFiles($"*{FileMarks.CompressedImage}.*")
            .OrderBy(t => t.Name);

        return [.. compressedImages];
    }
}
