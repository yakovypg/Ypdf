using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.CommandLine.Tools;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class ConfigToolCreator : IToolCreator
{
    public IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        ConfigSubcommand subcommand = config.ConfigSubcommand;

        ITool tool;

        if (subcommand.SaveConfig)
        {
            GlobalConfig newGlobalConfig = GlobalConfig.Instance.Copy();

            if (!string.IsNullOrEmpty(subcommand.PythonAlias))
                newGlobalConfig.PythonAlias = subcommand.PythonAlias;

            tool = new ResetGlobalConfigTool(GlobalConfig.Instance, newGlobalConfig);
        }
        else if (subcommand.ResetConfig)
        {
            tool = new ResetGlobalConfigTool(GlobalConfig.Instance);
        }
        else
        {
            tool = new ShowGlobalConfigTool(GlobalConfig.Instance);
        }

        return new ToolExecutionProvider(
            tool,
            [FilePaths.Config],
            FilePaths.Config);
    }
}
