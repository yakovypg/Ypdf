using System.Collections.Generic;
using NetArgumentParser.Informing;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Creators;

namespace Ypdf.CommandLine.Execution;

internal sealed class ToolExecutor : IToolExecutor
{
    internal ToolExecutor(IReadOnlyDictionary<string, IToolCreator> tools)
    {
        ExtendedArgumentNullException.ThrowIfNull(tools, nameof(tools));
        Tools = tools;
    }

    internal IReadOnlyDictionary<string, IToolCreator> Tools { get; }

    public void Execute(YpdfParserConfig config, ParseArgumentsResult parseResult)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));
        ExtendedArgumentNullException.ThrowIfNull(parseResult, nameof(parseResult));

        bool hasSubcommand = parseResult.TryGetLastHandledSubcommand(out Subcommand? subcommand);

        if (!hasSubcommand || subcommand is null)
            throw new ToolNotSpecifiedException();

        bool isSubcommandRegistered = Tools
            .TryGetValue(subcommand.Name, out IToolCreator? toolCreator);

        if (!isSubcommandRegistered || toolCreator is null)
            throw new UnknownToolException(null, subcommand.Name);

        IToolExecutionProvider toolExecutionProvider = toolCreator.Create(config);
        toolExecutionProvider.ExecuteTool();
    }
}
