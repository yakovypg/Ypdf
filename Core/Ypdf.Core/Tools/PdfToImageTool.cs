using System;
using System.Collections.Generic;
using Ypdf.Core.Config;
using Ypdf.Core.Extensions;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Runtime.Python;

namespace Ypdf.Core.Tools;

public class PdfToImageTool : IMultipleInputTool, IMultipleOutputTool
{
    public PdfToImageTool(
        string? pythonAlias = null,
        int extractedImagesLimit = 0,
        IOutputWriter? outputWriter = null)
    {
        PythonAlias = pythonAlias;
        ExtractedImagesLimit = extractedImagesLimit;
        OutputWriter = outputWriter;
    }

    protected string? PythonAlias { get; init; }
    protected int ExtractedImagesLimit { get; init; }
    protected IOutputWriter? OutputWriter { get; init; }

    public void Execute(string inputPath, string outputPath)
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

        string paths = string.Join(" ", inputPaths);
        string imageExtractorPath = PythonScriptPaths.ImageExtractor;

        var executor = new PythonExecutor()
        {
            OutputWriter = OutputWriter,
            RequirePython3 = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            ThrowExceptionIfExitWithError = true,
            ErrorDataVerifier = t => !string.IsNullOrEmpty(t),

            ErrorDataConverter = t => t is not null && t.Contains("pages", StringComparison.InvariantCulture)
                ? $"\n{t}" : t
        };

        if (PythonAlias is not null)
            executor.PythonAlias = PythonAlias;

        executor.Execute($"{imageExtractorPath} -l {ExtractedImagesLimit} -o {outputPath} -i {paths}");
    }
}
