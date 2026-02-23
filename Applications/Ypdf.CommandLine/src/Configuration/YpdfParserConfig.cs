using NetArgumentParser.Attributes;

namespace Ypdf.CommandLine.Configuration;

[ParserConfig]
internal sealed class YpdfParserConfig
{
    internal YpdfParserConfig()
    {
        AddPageNumbersSubcommand = new();
        AddWatermarkAnnotationSubcommand = new();
        AddWatermarkSubcommand = new();
        CompressImagesSubcommand = new();
        CompressSubcommand = new();
        ConfigSubcommand = new();
        CopySubcommand = new();
        CropPagesSubcommand = new();
        DividePagesSubcommand = new();
        ExtractImagesSubcommand = new();
        ExtractTextSubcommand = new();
        ImagesToPdfSubcommand = new();
        MergeSubcommand = new();
        MovePageSubcommand = new();
        RemovePagesSubcommand = new();
        RemovePasswordSubcommand = new();
        RemoveWatermarkAnnotationSubcommand = new();
        ReorderPagesSubcommand = new();
        RotatePagesSubcommand = new();
        SetPasswordSubcommand = new();
        SplitSubcommand = new();
        TextToPdfSubcommand = new();
    }

    [Subcommand(SubcommandNames.AddPageNumbers, SubcommandDescriptions.AddPageNumbers)]
    internal AddPageNumbersSubcommand AddPageNumbersSubcommand { get; }

    [Subcommand(SubcommandNames.AddWatermarkAnnotation, SubcommandDescriptions.AddWatermarkAnnotation)]
    internal AddWatermarkAnnotationSubcommand AddWatermarkAnnotationSubcommand { get; }

    [Subcommand(SubcommandNames.AddWatermark, SubcommandDescriptions.AddWatermark)]
    internal AddWatermarkSubcommand AddWatermarkSubcommand { get; }

    [Subcommand(SubcommandNames.CompressImages, SubcommandDescriptions.CompressImages)]
    internal CompressImagesSubcommand CompressImagesSubcommand { get; }

    [Subcommand(SubcommandNames.Compress, SubcommandDescriptions.Compress)]
    internal CompressSubcommand CompressSubcommand { get; }

    [Subcommand(SubcommandNames.Config, SubcommandDescriptions.Config)]
    internal ConfigSubcommand ConfigSubcommand { get; }

    [Subcommand(SubcommandNames.Copy, SubcommandDescriptions.Copy)]
    internal CopySubcommand CopySubcommand { get; }

    [Subcommand(SubcommandNames.CropPages, SubcommandDescriptions.CropPages)]
    internal CropPagesSubcommand CropPagesSubcommand { get; }

    [Subcommand(SubcommandNames.DividePages, SubcommandDescriptions.DividePages)]
    internal DividePagesSubcommand DividePagesSubcommand { get; }

    [Subcommand(SubcommandNames.ExtractImages, SubcommandDescriptions.ExtractImages)]
    internal ExtractImagesSubcommand ExtractImagesSubcommand { get; }

    [Subcommand(SubcommandNames.ExtractText, SubcommandDescriptions.ExtractText)]
    internal ExtractTextSubcommand ExtractTextSubcommand { get; }

    [Subcommand(SubcommandNames.ImagesToPdf, SubcommandDescriptions.ImagesToPdf)]
    internal ImagesToPdfSubcommand ImagesToPdfSubcommand { get; }

    [Subcommand(SubcommandNames.Merge, SubcommandDescriptions.Merge)]
    internal MergeSubcommand MergeSubcommand { get; }

    [Subcommand(SubcommandNames.MovePage, SubcommandDescriptions.MovePage)]
    internal MovePageSubcommand MovePageSubcommand { get; }

    [Subcommand(SubcommandNames.RemovePages, SubcommandDescriptions.RemovePages)]
    internal RemovePagesSubcommand RemovePagesSubcommand { get; }

    [Subcommand(SubcommandNames.RemovePassword, SubcommandDescriptions.RemovePassword)]
    internal RemovePasswordSubcommand RemovePasswordSubcommand { get; }

    [Subcommand(SubcommandNames.RemoveWatermarkAnnotation, SubcommandDescriptions.RemoveWatermarkAnnotation)]
    internal RemoveWatermarkAnnotationSubcommand RemoveWatermarkAnnotationSubcommand { get; }

    [Subcommand(SubcommandNames.ReorderPages, SubcommandDescriptions.ReorderPages)]
    internal ReorderPagesSubcommand ReorderPagesSubcommand { get; }

    [Subcommand(SubcommandNames.RotatePages, SubcommandDescriptions.RotatePages)]
    internal RotatePagesSubcommand RotatePagesSubcommand { get; }

    [Subcommand(SubcommandNames.SetPassword, SubcommandDescriptions.SetPassword)]
    internal SetPasswordSubcommand SetPasswordSubcommand { get; }

    [Subcommand(SubcommandNames.Split, SubcommandDescriptions.Split)]
    internal SplitSubcommand SplitSubcommand { get; }

    [Subcommand(SubcommandNames.TextToPdf, SubcommandDescriptions.TextToPdf)]
    internal TextToPdfSubcommand TextToPdfSubcommand { get; }
}
