using System.Collections.Generic;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Creators.Tools;

namespace Ypdf.CommandLine.Creators;

internal sealed class SupportedToolsCreator : ISupportedToolsCreator
{
    public Dictionary<string, IToolCreator> Create()
    {
        return new()
        {
            // TODO
            { AddPageNumbersSubcommand.Name, new AddPageNumbersToolCreator() },
            { AddWatermarkAnnotationSubcommand.Name, new AddWatermarkAnnotationToolCreator() },
            { AddWatermarkSubcommand.Name, new AddWatermarkToolCreator() },
            { CheckCompressionCapabilitySubcommand.Name, new CheckCompressionCapabilityToolCreator() },
            { CompressImagesSubcommand.Name, new CompressImageToolCreator() },
            { CompressSubcommand.Name, new CompressToolCreator() },
            { ConfigSubcommand.Name, new ConfigToolCreator() },
            { CopySubcommand.Name, new CopyToolCreator() },
            { CropPageSubcommand.Name, new CropPageToolCreator() },
            { DividePageSubcommand.Name, new DividePageToolCreator() },
            /*{ ExtractImagesSubcommand.Name, new ExtractImagesToolCreator() },
            { ExtractTextSubcommand.Name, new ExtractTextToolCreator() },
            { ImagesToPdfSubcommand.Name, new ImagesToPdfToolCreator() },*/
            { MergeSubcommand.Name, new MergeToolCreator() },
            { MovePageSubcommand.Name, new MovePageToolCreator() },
            /*{ RemovePagesSubcommand.Name, new RemovePagesToolCreator() },
            { RemovePasswordSubcommand.Name, new RemovePasswordToolCreator() },
            { RemoveWatermarkAnnotationSubcommand.Name, new RemoveWatermarkAnnotationToolCreator() },
            { ReorderPagesSubcommand.Name, new ReorderPagesToolCreator() },
            { RotatePagesSubcommand.Name, new RotatePagesToolCreator() },
            { SetPasswordSubcommand.Name, new SetPasswordToolCreator() },
            { SplitSubcommand.Name, new SplitToolCreator() },
            { TextToPdfSubcommand.Name, new TextToPdfToolCreator() },*/
        };
    }
}
