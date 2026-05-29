using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Execution;
using Ypdf.Core.Imaging;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class CompressImageToolCreator : ToolCreator
{
    public CompressImageToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        CompressImagesSubcommand subcommand = config.CompressImageSubcommand;

        var imageCompression = subcommand.Width is not null && subcommand.Height is not null
            ? new ImageCompression(
                subcommand.QualityFactor,
                subcommand.Width.Value,
                subcommand.Height.Value,
                subcommand.Extension)
            : new ImageCompression(
                subcommand.QualityFactor,
                subcommand.SizeFactor,
                subcommand.Extension);

        string pythonAlias = Config.PythonAlias;
        string virtualEnvironmentPath = Config.VirtualEnvironmentPath;
        IOutputWriter outputWriter = Config.OutputWriter;

        var tool = new CompressImageTool(imageCompression, pythonAlias, virtualEnvironmentPath, outputWriter);

        var toolExecutionParameters = new ToolExecutionParameters(
            tool,
            subcommand.InputPaths,
            subcommand.OutputPath,
            subcommand.InputPaths.Count == 1);

        return new ToolExecutionProvider(toolExecutionParameters);
    }
}
