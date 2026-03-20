using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;

namespace Ypdf.CommandLine.Creators.Tools;

internal abstract class ToolCreator : IToolCreator
{
    protected ToolCreator(GlobalConfig globalConfig)
    {
        ExtendedArgumentNullException.ThrowIfNull(globalConfig, nameof(globalConfig));
        Config = globalConfig;
    }

    protected GlobalConfig Config { get; }

    public abstract IToolExecutionProvider Create(YpdfParserConfig config);
}
