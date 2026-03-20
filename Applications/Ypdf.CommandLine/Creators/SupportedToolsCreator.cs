using System.Collections.Generic;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Creators.Tools;

namespace Ypdf.CommandLine.Creators;

internal sealed class SupportedToolsCreator : ISupportedToolsCreator
{
    public Dictionary<string, IToolCreator> Create(GlobalConfig globalConfig)
    {
        ExtendedArgumentNullException.ThrowIfNull(globalConfig, nameof(globalConfig));

        return new()
        {
            { AddPageNumbersSubcommand.Name, new AddPageNumbersToolCreator(globalConfig) },
            { AddWatermarkAnnotationSubcommand.Name, new AddWatermarkAnnotationToolCreator(globalConfig) },
            { AddWatermarkSubcommand.Name, new AddWatermarkToolCreator(globalConfig) },
            { CheckCompressionCapabilitySubcommand.Name, new CheckCompressionCapabilityToolCreator(globalConfig) },
            { CompressImagesSubcommand.Name, new CompressImageToolCreator(globalConfig) },
            { CompressSubcommand.Name, new CompressToolCreator(globalConfig) },
            { ConfigSubcommand.Name, new ConfigToolCreator(globalConfig) },
            { CopySubcommand.Name, new CopyToolCreator(globalConfig) },
            { CropPagesSubcommand.Name, new CropPageToolCreator(globalConfig) },
            { DividePagesSubcommand.Name, new DividePageToolCreator(globalConfig) },
            { ExtractImagesSubcommand.Name, new ExtractImagesToolCreator(globalConfig) },
            { ExtractTextSubcommand.Name, new ExtractTextToolCreator(globalConfig) },
            { ImagesToPdfSubcommand.Name, new ImagesToPdfToolCreator(globalConfig) },
            { MergeSubcommand.Name, new MergeToolCreator(globalConfig) },
            { MovePageSubcommand.Name, new MovePageToolCreator(globalConfig) },
            { RemovePagesSubcommand.Name, new RemovePageToolCreator(globalConfig) },
            { RemovePasswordSubcommand.Name, new RemovePasswordToolCreator(globalConfig) },
            { RemoveWatermarkAnnotationSubcommand.Name, new RemoveWatermarkAnnotationToolCreator(globalConfig) },
            { ReorderPagesSubcommand.Name, new ReorderPagesToolCreator(globalConfig) },
            { RotatePagesSubcommand.Name, new RotatePagesToolCreator(globalConfig) },
            { SetPasswordSubcommand.Name, new SetPasswordToolCreator(globalConfig) },
            { SplitSubcommand.Name, new SplitToolCreator(globalConfig) },
            { TextToPdfSubcommand.Name, new TextToPdfToolCreator(globalConfig) }
        };
    }
}
