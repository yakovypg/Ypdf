using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Execution;
using Ypdf.Core.Imaging;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class CompressToolCreator : ToolCreator
{
    public CompressToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        CompressSubcommand subcommand = config.CompressSubcommand;

        var imageCompression = new ImageCompression(
            subcommand.QualityFactor,
            subcommand.SizeFactor,
            subcommand.Extension);

        bool checkCompressionCapability = !subcommand.DisableCompressionCapabilityCheck;
        string pythonAlias = Config.PythonAlias;
        string virtualEnvironmentPath = Config.VirtualEnvironmentPath;
        IOutputWriter outputWriter = Config.OutputWriter;

        var tool = new CompressTool(
            imageCompression,
            checkCompressionCapability,
            pythonAlias,
            virtualEnvironmentPath,
            outputWriter);

        var toolExecutionParameters = new ToolExecutionParameters(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);

        return new ToolExecutionProvider(toolExecutionParameters);
    }
}
