using System.Collections.Generic;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Configuration.Restrictions;

namespace Ypdf.CommandLine.Creators;

internal sealed class SupportedOptionRestrictionProvidersCreator : ISupportedOptionRestrictionProvidersCreator
{
    public Dictionary<string, IOptionRestrictionProvider> Create()
    {
        return new()
        {
            { AddPageNumbersSubcommand.Name, new AddPageNumbersSubcommandOptionRestrictions() },
            { AddWatermarkAnnotationSubcommand.Name, new AddWatermarkAnnotationSubcommandOptionRestrictions() },
            { AddWatermarkSubcommand.Name, new AddWatermarkSubcommandOptionRestrictions() },
            { CheckCompressionCapabilitySubcommand.Name, new CheckCompressionCapabilitySubcommandOptionRestrictions() },
            { CompressImagesSubcommand.Name, new CompressImagesSubcommandOptionRestrictions() },
            { CompressSubcommand.Name, new CompressSubcommandOptionRestrictions() },
            { ConfigSubcommand.Name, new ConfigSubcommandOptionRestrictions() },
            { CopySubcommand.Name, new CopySubcommandOptionRestrictions() },
            { CropPageSubcommand.Name, new CropPagesSubcommandOptionRestrictions() },
            { DividePageSubcommand.Name, new DividePagesSubcommandOptionRestrictions() },
            { ExtractImagesSubcommand.Name, new ExtractImagesSubcommandOptionRestrictions() },
            { ExtractTextSubcommand.Name, new ExtractTextSubcommandOptionRestrictions() },
            { ImagesToPdfSubcommand.Name, new ImagesToPdfSubcommandOptionRestrictions() },
            { MergeSubcommand.Name, new MergeSubcommandOptionRestrictions() },
            { MovePageSubcommand.Name, new MovePageSubcommandOptionRestrictions() },
            { RemovePagesSubcommand.Name, new RemovePagesSubcommandOptionRestrictions() },
            { RemovePasswordSubcommand.Name, new RemovePasswordSubcommandOptionRestrictions() },
            { RemoveWatermarkAnnotationSubcommand.Name, new RemoveWatermarkAnnotationSubcommandOptionRestrictions() },
            { ReorderPagesSubcommand.Name, new ReorderPagesSubcommandOptionRestrictions() },
            { RotatePagesSubcommand.Name, new RotatePagesSubcommandOptionRestrictions() },
            { SetPasswordSubcommand.Name, new SetPasswordSubcommandOptionRestrictions() },
            { SplitSubcommand.Name, new SplitSubcommandOptionRestrictions() },
            { TextToPdfSubcommand.Name, new TextToPdfSubcommandOptionRestrictions() },
        };
    }
}
