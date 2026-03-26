using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class ExtractTextToolCreator : ToolCreator
{
    public ExtractTextToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        ExtractTextSubcommand subcommand = config.ExtractTextSubcommand;

        string pythonAlias = Config.PythonAlias;
        string virtualEnvironmentPath = Config.VirtualEnvironmentPath;
        IOutputWriter outputWriter = Config.OutputWriter;

        ITool tool = subcommand.UseTika
            ? new PdfToTextTool(pythonAlias, virtualEnvironmentPath, outputWriter)
            : new PdfToTextSimpleTool(subcommand.TextExtractor);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
