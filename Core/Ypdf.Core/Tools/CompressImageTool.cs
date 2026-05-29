using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ypdf.Core.Config;
using Ypdf.Core.Extensions;
using Ypdf.Core.Imaging;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Runtime.Python;

namespace Ypdf.Core.Tools;

public class CompressImageTool : PythonTool, IMultipleInputTool, IMultipleOutputTool
{
    public CompressImageTool(
        string? pythonAlias = null,
        string? virtualEnvironmentPath = null,
        IOutputWriter? outputWriter = null)
        : this(new ImageCompression(), pythonAlias, virtualEnvironmentPath, outputWriter) { }

    public CompressImageTool(
        ImageCompression imageCompression,
        string? pythonAlias = null,
        string? virtualEnvironmentPath = null,
        IOutputWriter? outputWriter = null)
        : base(pythonAlias, virtualEnvironmentPath, outputWriter)
    {
        ImageCompression = imageCompression;
    }

    protected ImageCompression ImageCompression { get; }

    protected override IEnumerable<PythonPackage> VirtualEnvironmentPackages =>
    [
        new("pillow", "12.1.1")
    ];

    public override void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        string imageCompressorPath = PythonScriptPaths.ImageCompressor;
        string sizeFactor = ImageCompression.SizeFactor.ToString(CultureInfo.InvariantCulture);
        string qualityFactor = ImageCompression.QualityFactor.ToString(CultureInfo.InvariantCulture);

        inputPath = inputPath.Quoted();
        outputPath = outputPath.Quoted();

        string args = $"{imageCompressorPath} -i {inputPath} -o {outputPath} -q {qualityFactor}";

        if (ImageCompression.NewWidth is not null && ImageCompression.NewHeight is not null)
            args += $" -W {ImageCompression.NewWidth} -H {ImageCompression.NewHeight}";
        else
            args += $" -s {sizeFactor}";

        ExecutePython(args);
    }

    public void Execute(IEnumerable<string> inputPaths, string outputPath)
    {
        ExtendedArgumentNullException.ThrowIfNull(inputPaths, nameof(inputPaths));
        DefaultExceptions.ThrowIfContainsNotExistingFile(inputPaths, nameof(inputPaths));

        if (string.IsNullOrWhiteSpace(outputPath))
            outputPath = "\"\"";
        else
            outputPath = outputPath.Quoted();

        inputPaths = inputPaths.Select(t => t.Quoted());

        string imageCompressorPath = PythonScriptPaths.ImageCompressor.Quoted();
        string inputPathsString = string.Join(" ", inputPaths);

        string sizeFactor = ImageCompression.SizeFactor.ToString(CultureInfo.InvariantCulture);
        string qualityFactor = ImageCompression.QualityFactor.ToString(CultureInfo.InvariantCulture);

        string args =
            $"{imageCompressorPath} -i {inputPathsString} -o {outputPath} " +
            $"-q {qualityFactor} -s {sizeFactor}";

        if (!string.IsNullOrEmpty(ImageCompression.Extension))
            args += $" -e {ImageCompression.Extension}";

        ExecutePython(args);
    }
}
