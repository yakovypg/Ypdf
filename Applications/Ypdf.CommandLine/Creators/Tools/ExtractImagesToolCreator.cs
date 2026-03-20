using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class ExtractImagesToolCreator : IToolCreator
{
    public IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        ExtractImagesSubcommand subcommand = config.ExtractImagesSubcommand;

        string? pythonAlias = GlobalConfig.Instance.PythonAlias;
        IOutputWriter outputWriter = GlobalConfig.Instance.OutputWriter;

        PdfToImageTool tool = new(pythonAlias, subcommand.ExtractedImagesLimit, outputWriter);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
