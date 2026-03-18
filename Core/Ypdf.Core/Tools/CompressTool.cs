using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using Ypdf.Core.Compression;
using Ypdf.Core.Extensions;
using Ypdf.Core.FileSystem.Naming;
using Ypdf.Core.FileSystem.Paths;
using Ypdf.Core.Imaging;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Paths;

namespace Ypdf.Core.Tools;

public class CompressTool : ITool
{
    public CompressTool(
        ImageCompression imageCompression = default,
        string? pythonAlias = null,
        bool checkCompressionCapability = true,
        IOutputWriter? outputWriter = null)
    {
        ImageCompression = imageCompression;
        PythonAlias = pythonAlias;
        CheckCompressionCapability = checkCompressionCapability;
        OutputWriter = outputWriter;
    }

    protected ImageCompression ImageCompression { get; }
    protected string? PythonAlias { get; }
    protected bool CheckCompressionCapability { get; }
    protected IOutputWriter? OutputWriter { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        DirectoryInfo uniqueDirectory = UniqueDirectory.Create(Directories.Temp);

        if (CheckCompressionCapability)
            VerifyCompressionCapability(inputPath);

        IEnumerable<string> extractedImagePaths = ExtractImages(inputPath, uniqueDirectory);

        IList<string> compressedImagePaths =
            CompressImages(extractedImagePaths, uniqueDirectory)
            .ToList();

        bool imageInserted = InsertImages(inputPath, outputPath, compressedImagePaths);

        if (!imageInserted)
            throw new CompressionException("Failed to insert compressed images.");

        PrintCompressionSummary(inputPath, outputPath);
        Directory.Delete(uniqueDirectory.FullName, true);
    }

    protected virtual void VerifyCompressionCapability(string inputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        OutputWriter?.WriteLine("Compression capability is being checked...");

        var checkingTool = new CheckCompressionCapabilityTool(PythonAlias, OutputWriter);
        bool canCompress = checkingTool.Execute(inputPath);

        if (!canCompress)
            throw new CompressionNotPossibleException();

        const string message =
            "Compression check is complete. " +
            "Document can be compressed.";

        OutputWriter?.WriteLine(message);
    }

    protected virtual void PrintCompressionSummary(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        if (OutputWriter is null)
            return;

        long inputFileSize = new FileInfo(inputPath).Length;
        long outputFileSize = new FileInfo(outputPath).Length;

        double inputFileSizeKb = inputFileSize / 1024.0;
        double outputFileSizeKb = outputFileSize / 1024.0;
        double compression = (1 - (outputFileSize / (double)inputFileSize)) * 100;

        double inputFileSizeKbRounded = Math.Round(inputFileSizeKb, 5);
        double outputFileSizeKbRounded = Math.Round(outputFileSizeKb, 5);
        double compressionRounded = Math.Round(compression, 5);

        OutputWriter.WriteLine();
        OutputWriter.WriteLine("Compression summary");
        OutputWriter.WriteLine($"Input document size: {inputFileSizeKbRounded} KB");
        OutputWriter.WriteLine($"Output document size: {outputFileSizeKbRounded} KB");
        OutputWriter.WriteLine($"Compression: {compressionRounded}%");
    }

    private static bool InsertImages(
        string inputPath,
        string outputPath,
        IList<string> compressedImagePaths)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        ExtendedArgumentNullException.ThrowIfNull(compressedImagePaths, nameof(compressedImagePaths));
        DefaultExceptions.ThrowIfContainsNotExistingFile(compressedImagePaths, nameof(compressedImagePaths));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        using var reader = new PdfReader(inputPath);
        using var writer = new PdfWriter(outputPath);
        using var pdfDocument = new PdfDocument(reader, writer);

        int imageIndex = 0;
        int numOfPages = pdfDocument.GetNumberOfPages();

        for (int i = 1; i <= numOfPages; ++i)
        {
            PdfPage currPage = pdfDocument.GetPage(i);

            IList<PdfName> imageReferences =
                currPage.GetImageReferences(out PdfDictionary? resourcesXObjects);

            foreach (PdfName imageReference in imageReferences)
            {
                if (imageIndex >= compressedImagePaths.Count)
                    return false;

                string compressedImagePath = compressedImagePaths[imageIndex++];

                ImageData compressedImageData = ImageDataFactory.Create(compressedImagePath);
                Image compressedImage = new(compressedImageData);

                PdfStream compressedImageStream = compressedImage
                    .GetXObject()
                    .GetPdfObject();

                resourcesXObjects?.Put(imageReference, compressedImageStream);
            }
        }

        return true;
    }

    private List<string> ExtractImages(string inputPath, DirectoryInfo uniqueDirectory)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentNullException.ThrowIfNull(uniqueDirectory, nameof(uniqueDirectory));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        var pdfToImageTool = new PdfToImageTool(PythonAlias, 0, OutputWriter);
        pdfToImageTool.Execute(inputPath, uniqueDirectory.FullName);

        IEnumerable<string> extractedImagePaths = uniqueDirectory
            .GetFiles()
            .Select(t => t.FullName)
            .OrderBy(t => PathExplorer.GetImageNumberFromPath(t));

        return new List<string>(extractedImagePaths);
    }

    private List<string> CompressImages(
        IEnumerable<string> inputPaths,
        DirectoryInfo uniqueDirectory)
    {
        ExtendedArgumentNullException.ThrowIfNull(inputPaths, nameof(inputPaths));
        DefaultExceptions.ThrowIfContainsNotExistingFile(inputPaths, nameof(inputPaths));
        ExtendedArgumentNullException.ThrowIfNull(uniqueDirectory, nameof(uniqueDirectory));

        var compressImageTool = new CompressImageTool(ImageCompression, PythonAlias, OutputWriter);
        var inputPathsSequence = new PathSequence(inputPaths);

        IEnumerable<IEnumerable<string>> inputPathGroups = inputPathsSequence.Group();

        foreach (var part in inputPathGroups)
        {
            compressImageTool.Execute(part, uniqueDirectory.FullName);
        }

        IEnumerable<string> compressedImagePaths = uniqueDirectory
            .GetFiles($"*{FileMarks.CompressedImage}.*")
            .Select(t => t.FullName)
            .OrderBy(t => PathExplorer.GetImageNumberFromPath(t));

        return new List<string>(compressedImagePaths);
    }
}
