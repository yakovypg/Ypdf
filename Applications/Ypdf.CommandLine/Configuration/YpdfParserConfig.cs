using NetArgumentParser.Attributes;

namespace Ypdf.CommandLine.Configuration;

[ParserConfig]
internal sealed class YpdfParserConfig
{
    [Subcommand(AddPageNumbersSubcommand.Name, AddPageNumbersSubcommand.Description)]
    internal AddPageNumbersSubcommand AddPageNumbersSubcommand { get; } = new();

    [Subcommand(AddWatermarkAnnotationSubcommand.Name, AddWatermarkAnnotationSubcommand.Description)]
    internal AddWatermarkAnnotationSubcommand AddWatermarkAnnotationSubcommand { get; } = new();

    [Subcommand(AddWatermarkSubcommand.Name, AddWatermarkSubcommand.Description)]
    internal AddWatermarkSubcommand AddWatermarkSubcommand { get; } = new();

    [Subcommand(CheckCompressionCapabilitySubcommand.Name, CheckCompressionCapabilitySubcommand.Description)]
    internal CheckCompressionCapabilitySubcommand CheckCompressionCapabilitySubcommand { get; } = new();

    [Subcommand(CompressImagesSubcommand.Name, CompressImagesSubcommand.Description)]
    internal CompressImagesSubcommand CompressImagesSubcommand { get; } = new();

    [Subcommand(CompressSubcommand.Name, CompressSubcommand.Description)]
    internal CompressSubcommand CompressSubcommand { get; } = new();

    [Subcommand(ConfigSubcommand.Name, ConfigSubcommand.Description)]
    internal ConfigSubcommand ConfigSubcommand { get; } = new();

    [Subcommand(CopySubcommand.Name, CopySubcommand.Description)]
    internal CopySubcommand CopySubcommand { get; } = new();

    [Subcommand(CropPageSubcommand.Name, CropPageSubcommand.Description)]
    internal CropPageSubcommand CropPageSubcommand { get; } = new();

    [Subcommand(DividePageSubcommand.Name, DividePageSubcommand.Description)]
    internal DividePageSubcommand DividePageSubcommand { get; } = new();

    [Subcommand(ExtractImagesSubcommand.Name, ExtractImagesSubcommand.Description)]
    internal ExtractImagesSubcommand ExtractImagesSubcommand { get; } = new();

    [Subcommand(ExtractTextSubcommand.Name, ExtractTextSubcommand.Description)]
    internal ExtractTextSubcommand ExtractTextSubcommand { get; } = new();

    [Subcommand(ImagesToPdfSubcommand.Name, ImagesToPdfSubcommand.Description)]
    internal ImagesToPdfSubcommand ImagesToPdfSubcommand { get; } = new();

    [Subcommand(MergeSubcommand.Name, MergeSubcommand.Description)]
    internal MergeSubcommand MergeSubcommand { get; } = new();

    [Subcommand(MovePageSubcommand.Name, MovePageSubcommand.Description)]
    internal MovePageSubcommand MovePageSubcommand { get; } = new();

    [Subcommand(RemovePagesSubcommand.Name, RemovePagesSubcommand.Description)]
    internal RemovePagesSubcommand RemovePagesSubcommand { get; } = new();

    [Subcommand(RemovePasswordSubcommand.Name, RemovePasswordSubcommand.Description)]
    internal RemovePasswordSubcommand RemovePasswordSubcommand { get; } = new();

    [Subcommand(RemoveWatermarkAnnotationSubcommand.Name, RemoveWatermarkAnnotationSubcommand.Description)]
    internal RemoveWatermarkAnnotationSubcommand RemoveWatermarkAnnotationSubcommand { get; } = new();

    [Subcommand(ReorderPagesSubcommand.Name, ReorderPagesSubcommand.Description)]
    internal ReorderPagesSubcommand ReorderPagesSubcommand { get; } = new();

    [Subcommand(RotatePagesSubcommand.Name, RotatePagesSubcommand.Description)]
    internal RotatePagesSubcommand RotatePagesSubcommand { get; } = new();

    [Subcommand(SetPasswordSubcommand.Name, SetPasswordSubcommand.Description)]
    internal SetPasswordSubcommand SetPasswordSubcommand { get; } = new();

    [Subcommand(SplitSubcommand.Name, SplitSubcommand.Description)]
    internal SplitSubcommand SplitSubcommand { get; } = new();

    [Subcommand(TextToPdfSubcommand.Name, TextToPdfSubcommand.Description)]
    internal TextToPdfSubcommand TextToPdfSubcommand { get; } = new();
}
