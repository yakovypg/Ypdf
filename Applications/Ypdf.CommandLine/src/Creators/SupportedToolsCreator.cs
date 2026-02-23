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
            { SubcommandNames.AddPageNumbers, new AddPageNumbersToolCreator() },
            /*{ SubcommandNames.AddWatermarkAnnotation, new AddWatermarkAnnotationToolCreator() },
            { SubcommandNames.AddWatermark, new AddWatermarkToolCreator() },
            { SubcommandNames.CompressImages, new CompressImagesToolCreator() },
            { SubcommandNames.Compress, new CompressToolCreator() },
            { SubcommandNames.Config, new ConfigToolCreator() },
            { SubcommandNames.Copy, new CopyToolCreator() },
            { SubcommandNames.CropPages, new CropPagesToolCreator() },
            { SubcommandNames.DividePages, new DividePagesToolCreator() },
            { SubcommandNames.ExtractImages, new ExtractImagesToolCreator() },
            { SubcommandNames.ExtractText, new ExtractTextToolCreator() },
            { SubcommandNames.ImagesToPdf, new ImagesToPdfToolCreator() },
            { SubcommandNames.Merge, new MergeToolCreator() },
            { SubcommandNames.MovePage, new MovePageToolCreator() },
            { SubcommandNames.RemovePages, new RemovePagesToolCreator() },
            { SubcommandNames.RemovePassword, new RemovePasswordToolCreator() },
            { SubcommandNames.RemoveWatermarkAnnotation, new RemoveWatermarkAnnotationToolCreator() },
            { SubcommandNames.ReorderPages, new ReorderPagesToolCreator() },
            { SubcommandNames.RotatePages, new RotatePagesToolCreator() },
            { SubcommandNames.SetPassword, new SetPasswordToolCreator() },
            { SubcommandNames.Split, new SplitToolCreator() },
            { SubcommandNames.TextToPdf, new TextToPdfToolCreator() },*/
        };
    }
}
