using System.Collections.Generic;
using NetArgumentParser.Informing;
using NetArgumentParser.Subcommands;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Creators.Tools;
using Ypdf.CommandLine.Validation;

namespace Ypdf.CommandLine.Execution;

internal sealed class ToolExecutor : IToolExecutor
{
    internal ToolExecutor(
        IReadOnlyDictionary<string, IToolCreator> tools,
        IValidationPipeline validationPipeline)
    {
        ExtendedArgumentNullException.ThrowIfNull(tools, nameof(tools));
        ExtendedArgumentNullException.ThrowIfNull(validationPipeline, nameof(validationPipeline));

        Tools = tools;
        ValidationPipeline = validationPipeline;
    }

    internal IReadOnlyDictionary<string, IToolCreator> Tools { get; }
    internal IValidationPipeline ValidationPipeline { get; }

    public void Execute(YpdfParserConfig config, ParseArgumentsResult parseResult)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));
        ExtendedArgumentNullException.ThrowIfNull(parseResult, nameof(parseResult));

        bool hasSubcommand = parseResult.TryGetLastHandledSubcommand(out Subcommand? subcommand);

        if (!hasSubcommand || subcommand is null)
            throw new ToolNotSpecifiedException(null);

        bool isSubcommandRegistered = Tools
            .TryGetValue(subcommand.Name, out IToolCreator? toolCreator);

        if (!isSubcommandRegistered || toolCreator is null)
            throw new UnknownToolException(null, subcommand.Name);

        IToolExecutionProvider toolExecutionProvider = toolCreator.Create(config);
        ValidationResult validationResult = ValidationPipeline.Run(toolExecutionProvider);

        if (!validationResult.IsValid)
        {
            if (validationResult.Errors.Count > 0)
                throw new ValidationException(null, validationResult.Errors[0]);

            throw new ValidationException();
        }

        toolExecutionProvider.ExecuteTool();
    }
}
