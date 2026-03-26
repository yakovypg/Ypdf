using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class ExtractImagesToolCreator : ToolCreator
{
    public ExtractImagesToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        ExtractImagesSubcommand subcommand = config.ExtractImagesSubcommand;

        string pythonAlias = Config.PythonAlias;
        string virtualEnvironmentPath = Config.VirtualEnvironmentPath;
        IOutputWriter outputWriter = Config.OutputWriter;

        PdfToImageTool tool = new(
            subcommand.MaxNumberOfImagesToExtract,
            pythonAlias,
            virtualEnvironmentPath,
            outputWriter);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
