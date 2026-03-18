using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Execution;
using Ypdf.Core.Imaging;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Creators.Tools;

internal sealed class CompressImagesToolCreator : IToolCreator
{
    public IToolExecutionProvider Create(YpdfParserConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        CompressImagesSubcommand subcommand = config.CompressImagesSubcommand;

        var imageCompression = new ImageCompression();

        var tool = new CompressImageTool(imageCompression, null, null);

        return new ToolExecutionProvider(
            tool,
            subcommand.InputPaths,
            subcommand.OutputPath);
    }
}
