using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Security;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class SetPasswordToolCreator : IToolCreator
{
    public IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        SetPasswordSubcommand subcommand = config.SetPasswordSubcommand;

        string userPassword = subcommand.UserPassword
            ?? subcommand.CommonPassword
            ?? subcommand.OwnerPassword
            ?? string.Empty;

        string ownerPassword = subcommand.OwnerPassword
            ?? subcommand.CommonPassword
            ?? subcommand.UserPassword
            ?? string.Empty;

        PdfPassword pdfPassword = new(userPassword, ownerPassword, subcommand.EncryptionAlgorithm);
        SetPasswordTool tool = new(pdfPassword);

        return new ToolExecutionProvider(
            tool,
            [subcommand.InputPath],
            subcommand.OutputPath);
    }
}
