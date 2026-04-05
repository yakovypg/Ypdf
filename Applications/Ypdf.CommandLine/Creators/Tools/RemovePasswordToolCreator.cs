using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Execution;
using Ypdf.Core.Security;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class RemovePasswordToolCreator : ToolCreator
{
    public RemovePasswordToolCreator(GlobalConfig globalConfig)
        : base(globalConfig ?? throw new ArgumentNullException(nameof(globalConfig))) { }

    public override IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        RemovePasswordSubcommand subcommand = config.RemovePasswordSubcommand;

        string userPassword = subcommand.UserPassword
            ?? subcommand.CommonPassword
            ?? subcommand.OwnerPassword
            ?? string.Empty;

        string ownerPassword = subcommand.OwnerPassword
            ?? subcommand.CommonPassword
            ?? subcommand.UserPassword
            ?? string.Empty;

        PdfPassword pdfPassword = new(userPassword, ownerPassword);
        RemovePasswordTool tool = new(pdfPassword);

        ToolExecutionParameters toolExecutionParameters = new(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);

        return new ToolExecutionProvider(toolExecutionParameters);
    }
}
