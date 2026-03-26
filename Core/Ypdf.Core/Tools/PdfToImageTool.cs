using System;
using System.Collections.Generic;
using System.Linq;
using Ypdf.Core.Config;
using Ypdf.Core.Extensions;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Runtime.Python;

namespace Ypdf.Core.Tools;

public class PdfToImageTool : PythonTool, IMultipleInputTool, IMultipleOutputTool
{
    public PdfToImageTool(
        int extractedImagesLimit = 0,
        string? pythonAlias = null,
        string? virtualEnvironmentPath = null,
        IOutputWriter? outputWriter = null)
        : base(pythonAlias, virtualEnvironmentPath, outputWriter)
    {
        ExtractedImagesLimit = extractedImagesLimit;
    }

    protected int ExtractedImagesLimit { get; init; }

    protected override IEnumerable<PythonPackage> VirtualEnvironmentPackages =>
    [
        new("PyMuPDF", "1.27.2.2"),
        new("tqdm", "4.67.3")
    ];

    public override void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));
        DefaultExceptions.ThrowIfDirectoryNotExists(outputPath, nameof(outputPath));

        Execute(new string[] { inputPath }, outputPath);
    }

    public void Execute(IEnumerable<string> inputPaths, string outputPath)
    {
        ExtendedArgumentNullException.ThrowIfNull(inputPaths, nameof(inputPaths));
        ExtendedArgumentNullException.ThrowIfNull(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfContainsNotExistingFile(inputPaths, nameof(inputPaths));
        DefaultExceptions.ThrowIfDirectoryNotExists(outputPath, nameof(outputPath));

        if (string.IsNullOrEmpty(outputPath))
            outputPath = "\"\"";
        else
            outputPath = outputPath.Quoted();

        inputPaths = inputPaths.Select(t => t.Quoted());

        string paths = string.Join(" ", inputPaths);
        string imageExtractorPath = PythonScriptPaths.ImageExtractor.Quoted();
        string args = $"{imageExtractorPath} -l {ExtractedImagesLimit} -o {outputPath} -i {paths}";

        PythonExecutor executor = CreateDefaultPythonExecutor();
        executor.ErrorDataVerifier = t => !string.IsNullOrEmpty(t);

        executor.ErrorDataConverter = t =>
        {
            return t is not null && t.Contains(OutputMarks.PdfToImageToolPages, StringComparison.Ordinal)
                ? $"\n{t}"
                : t;
        };

        executor.Execute(args);
    }
}
