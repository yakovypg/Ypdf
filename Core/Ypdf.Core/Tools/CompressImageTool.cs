using System.Collections.Generic;
using System.Globalization;
using Ypdf.Core.Imaging;
using Ypdf.Paths;
using Ypdf.Runtime.Logging;
using Ypdf.Runtime.Python;

namespace Ypdf.Core.Tools;

public class CompressImageTool : IMultipleInputTool
{
    public CompressImageTool(
        ImageCompression imageCompression = default,
        string? pythonAlias = null,
        IOutputWriter? outputWriter = null)
    {
        ImageCompression = imageCompression;
        PythonAlias = pythonAlias;
        OutputWriter = outputWriter;
    }

    protected ImageCompression ImageCompression { get; }
    protected string? PythonAlias { get; }
    protected IOutputWriter? OutputWriter { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        var executor = new PythonExecutor()
        {
            OutputWriter = OutputWriter,
            RequirePython3 = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            ThrowExceptionIfExitWithError = true
        };

        if (!string.IsNullOrEmpty(PythonAlias))
            executor.PythonAlias = PythonAlias;

        string imageCompressorPath = PythonScripts.ImageCompressor;
        string sizeFactor = ImageCompression.SizeFactor.ToString(CultureInfo.InvariantCulture);
        string qualityFactor = ImageCompression.QualityFactor.ToString(CultureInfo.InvariantCulture);

        string args =
            $"{imageCompressorPath} -i {inputPath} -o {outputPath} " +
            $"-q {qualityFactor} -s {sizeFactor}";

        if (ImageCompression.NewWidth is not null)
            args += $" -W {ImageCompression.NewWidth}";

        if (ImageCompression.NewHeight is not null)
            args += $" -H {ImageCompression.NewHeight}";

        executor.Execute(args);
    }

    public void Execute(IEnumerable<string> inputPaths, string outputPath)
    {
        ExtendedArgumentNullException.ThrowIfNull(inputPaths, nameof(inputPaths));
        ExtendedArgumentNullException.ThrowIfNull(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfContainsNotExistingFile(inputPaths, nameof(inputPaths));
        DefaultExceptions.ThrowIfDirectoryNotExists(outputPath, nameof(outputPath));

        if (string.IsNullOrEmpty(outputPath))
            outputPath = "\"\"";

        var executor = new PythonExecutor()
        {
            OutputWriter = OutputWriter,
            RequirePython3 = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            ThrowExceptionIfExitWithError = true
        };

        if (!string.IsNullOrEmpty(PythonAlias))
            executor.PythonAlias = PythonAlias;

        if (string.IsNullOrEmpty(outputPath))
            outputPath = "\"\"";

        string imageCompressorPath = PythonScripts.ImageCompressor;
        string inputPathsString = string.Join(" ", inputPaths);

        string sizeFactor = ImageCompression.SizeFactor.ToString(CultureInfo.InvariantCulture);
        string qualityFactor = ImageCompression.QualityFactor.ToString(CultureInfo.InvariantCulture);

        string args =
            $"{imageCompressorPath} -i {inputPathsString} -o {outputPath} " +
            $"-q {qualityFactor} -s {sizeFactor}";

        if (!string.IsNullOrEmpty(ImageCompression.Extension))
            args += $" -e {ImageCompression.Extension}";

        executor.Execute(args);
    }
}
