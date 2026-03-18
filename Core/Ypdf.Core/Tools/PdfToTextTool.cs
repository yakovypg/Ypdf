using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Runtime.Python;
using Ypdf.Paths;

namespace Ypdf.Core.Tools;

public class PdfToTextTool : ITool
{
    public PdfToTextTool(string? pythonAlias = null, IOutputWriter? outputWriter = null)
    {
        PythonAlias = pythonAlias;
        OutputWriter = outputWriter;
    }

    protected string? PythonAlias { get; }
    protected IOutputWriter? OutputWriter { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        string textExtractorPath = PythonScripts.TextExtractor;

        var executor = new PythonExecutor()
        {
            OutputWriter = OutputWriter,
            RequirePython3 = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            ThrowExceptionIfExitWithError = true,
        };

        if (PythonAlias is not null)
            executor.PythonAlias = PythonAlias;

        executor.Execute($"{textExtractorPath} -i {inputPath} -o {outputPath}");
    }
}
