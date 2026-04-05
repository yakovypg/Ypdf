using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.CommandLine.Tools;
using Ypdf.Core.Execution;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class ConfigToolCreator : ToolCreator
{
    public ConfigToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        ConfigSubcommand subcommand = config.ConfigSubcommand;

        ITool tool;

        if (subcommand.SaveConfig)
        {
            GlobalConfig newGlobalConfig = Config.Copy();

            if (!string.IsNullOrWhiteSpace(subcommand.PythonAlias))
                newGlobalConfig.PythonAlias = subcommand.PythonAlias!;

            if (!string.IsNullOrWhiteSpace(subcommand.VirtualEnvironmentPath))
                newGlobalConfig.VirtualEnvironmentPath = subcommand.VirtualEnvironmentPath!;

            tool = new ResetGlobalConfigTool(Config, newGlobalConfig);
        }
        else if (subcommand.ResetConfig)
        {
            tool = new ResetGlobalConfigTool(Config);
        }
        else
        {
            tool = new ShowGlobalConfigTool(Config);
        }

        var toolExecutionParameters = new ToolExecutionParameters(
            tool,
            [FilePaths.Config],
            FilePaths.Config);

        return new ToolExecutionProvider(toolExecutionParameters);
    }
}
