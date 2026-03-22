using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class GetInfoToolCreator : ToolCreator
{
    public GetInfoToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        GetInfoSubcommand subcommand = config.GetInfoSubcommand;
        IOutputWriter outputWriter = Config.OutputWriter;
        GetInfoTool tool = new(subcommand.MaxPageSizesToPrint, outputWriter);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
