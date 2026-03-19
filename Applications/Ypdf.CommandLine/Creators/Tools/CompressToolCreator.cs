using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Imaging;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class CompressToolCreator : IToolCreator
{
    public IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        CompressSubcommand subcommand = config.CompressSubcommand;

        var imageCompression = new ImageCompression(
            subcommand.QualityFactor,
            subcommand.SizeFactor,
            subcommand.Extension);

        bool checkCompressionCapability = !subcommand.DisableCompressionCapabilityCheck;
        string? pythonAlias = GlobalConfig.Instance.PythonAlias;
        IOutputWriter outputWriter = GlobalConfig.Instance.OutputWriter;

        var tool = new CompressTool(
            imageCompression,
            pythonAlias,
            checkCompressionCapability,
            outputWriter);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
