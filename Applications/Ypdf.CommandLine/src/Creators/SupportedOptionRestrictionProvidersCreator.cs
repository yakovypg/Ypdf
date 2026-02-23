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
            { SubcommandNames.AddPageNumbers, new AddPageNumbersSubcommandOptionRestrictions() },
            { SubcommandNames.AddWatermarkAnnotation, new AddWatermarkAnnotationSubcommandOptionRestrictions() },
            { SubcommandNames.AddWatermark, new AddWatermarkSubcommandOptionRestrictions() },
            { SubcommandNames.CompressImages, new CompressImagesSubcommandOptionRestrictions() },
            { SubcommandNames.Compress, new CompressSubcommandOptionRestrictions() },
            { SubcommandNames.Config, new ConfigSubcommandOptionRestrictions() },
            { SubcommandNames.Copy, new CopySubcommandOptionRestrictions() },
            { SubcommandNames.CropPages, new CropPagesSubcommandOptionRestrictions() },
            { SubcommandNames.DividePages, new DividePagesSubcommandOptionRestrictions() },
            { SubcommandNames.ExtractImages, new ExtractImagesSubcommandOptionRestrictions() },
            { SubcommandNames.ExtractText, new ExtractTextSubcommandOptionRestrictions() },
            { SubcommandNames.ImagesToPdf, new ImagesToPdfSubcommandOptionRestrictions() },
            { SubcommandNames.Merge, new MergeSubcommandOptionRestrictions() },
            { SubcommandNames.MovePage, new MovePageSubcommandOptionRestrictions() },
            { SubcommandNames.RemovePages, new RemovePagesSubcommandOptionRestrictions() },
            { SubcommandNames.RemovePassword, new RemovePasswordSubcommandOptionRestrictions() },
            { SubcommandNames.RemoveWatermarkAnnotation, new RemoveWatermarkAnnotationSubcommandOptionRestrictions() },
            { SubcommandNames.ReoprderPages, new ReorderPagesSubcommandOptionRestrictions() },
            { SubcommandNames.RotatePages, new RotatePagesSubcommandOptionRestrictions() },
            { SubcommandNames.SetPassword, new SetPasswordSubcommandOptionRestrictions() },
            { SubcommandNames.Split, new SplitSubcommandOptionRestrictions() },
            { SubcommandNames.TextToPdf, new TextToPdfSubcommandOptionRestrictions() },
        };
    }
}
