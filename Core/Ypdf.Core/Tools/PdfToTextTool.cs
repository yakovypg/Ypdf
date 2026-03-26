using System.Collections.Generic;
using Ypdf.Core.Config;
using Ypdf.Core.Extensions;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Runtime.Python;

namespace Ypdf.Core.Tools;

public class PdfToTextTool : PythonTool, ITool
{
    public PdfToTextTool(
        string? pythonAlias = null,
        string? virtualEnvironmentPath = null,
        IOutputWriter? outputWriter = null)
        : base(pythonAlias, virtualEnvironmentPath, outputWriter) { }

    protected override IEnumerable<PythonPackage> VirtualEnvironmentPackages =>
    [
        new("tika", "3.1.0")
    ];

    public override void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        inputPath = inputPath.Quoted();
        outputPath = outputPath.Quoted();

        string textExtractorPath = PythonScriptPaths.TextExtractor.Quoted();
        string args = $"{textExtractorPath} -i {inputPath} -o {outputPath}";

        ExecutePython(args);
    }
}
