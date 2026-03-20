using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class ExtractTextToolCreator : IToolCreator
{
    public IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        ExtractTextSubcommand subcommand = config.ExtractTextSubcommand;

        string? pythonAlias = GlobalConfig.Instance.PythonAlias;
        IOutputWriter outputWriter = GlobalConfig.Instance.OutputWriter;

        ITool tool = subcommand.UseTika
            ? new PdfToTextTool(pythonAlias, outputWriter)
            : new PdfToTextSimpleTool(subcommand.TextExtractor);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
