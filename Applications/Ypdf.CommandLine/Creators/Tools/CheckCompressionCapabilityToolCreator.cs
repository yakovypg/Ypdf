using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class CheckCompressionCapabilityToolCreator : IToolCreator
{
    public IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        CheckCompressionCapabilitySubcommand subcommand = config.CheckCompressionCapabilitySubcommand;

        string? pythonAlias = GlobalConfig.Instance.PythonAlias;
        IOutputWriter outputWriter = GlobalConfig.Instance.OutputWriter;

        var tool = new CheckCompressionCapabilityTool(
            pythonAlias,
            outputWriter);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
