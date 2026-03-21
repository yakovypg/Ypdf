using NetArgumentParser.Attributes;

namespace Ypdf.CommandLine.Configuration;

[ParserConfig]
internal sealed class YpdfParserConfig
{
    [Subcommand(AddPageNumbersSubcommand.Name, AddPageNumbersSubcommand.Description)]
    public AddPageNumbersSubcommand AddPageNumbersSubcommand { get; } = new();

    [Subcommand(AddWatermarkAnnotationSubcommand.Name, AddWatermarkAnnotationSubcommand.Description)]
    public AddWatermarkAnnotationSubcommand AddWatermarkAnnotationSubcommand { get; } = new();

    [Subcommand(AddWatermarkSubcommand.Name, AddWatermarkSubcommand.Description)]
    public AddWatermarkSubcommand AddWatermarkSubcommand { get; } = new();

    [Subcommand(CheckCompressionCapabilitySubcommand.Name, CheckCompressionCapabilitySubcommand.Description)]
    public CheckCompressionCapabilitySubcommand CheckCompressionCapabilitySubcommand { get; } = new();

    [Subcommand(CompressImagesSubcommand.Name, CompressImagesSubcommand.Description)]
    public CompressImagesSubcommand CompressImageSubcommand { get; } = new();

    [Subcommand(CompressSubcommand.Name, CompressSubcommand.Description)]
    public CompressSubcommand CompressSubcommand { get; } = new();

    [Subcommand(ConfigSubcommand.Name, ConfigSubcommand.Description)]
    public ConfigSubcommand ConfigSubcommand { get; } = new();

    [Subcommand(CopySubcommand.Name, CopySubcommand.Description)]
    public CopySubcommand CopySubcommand { get; } = new();

    [Subcommand(CropPagesSubcommand.Name, CropPagesSubcommand.Description)]
    public CropPagesSubcommand CropPageSubcommand { get; } = new();

    [Subcommand(DividePagesSubcommand.Name, DividePagesSubcommand.Description)]
    public DividePagesSubcommand DividePageSubcommand { get; } = new();

    [Subcommand(ExtractImagesSubcommand.Name, ExtractImagesSubcommand.Description)]
    public ExtractImagesSubcommand ExtractImagesSubcommand { get; } = new();

    [Subcommand(ExtractTextSubcommand.Name, ExtractTextSubcommand.Description)]
    public ExtractTextSubcommand ExtractTextSubcommand { get; } = new();

    [Subcommand(GetInfoSubcommand.Name, GetInfoSubcommand.Description)]
    public GetInfoSubcommand GetInfoSubcommand { get; } = new();

    [Subcommand(ImagesToPdfSubcommand.Name, ImagesToPdfSubcommand.Description)]
    public ImagesToPdfSubcommand ImagesToPdfSubcommand { get; } = new();

    [Subcommand(MergeSubcommand.Name, MergeSubcommand.Description)]
    public MergeSubcommand MergeSubcommand { get; } = new();

    [Subcommand(MovePageSubcommand.Name, MovePageSubcommand.Description)]
    public MovePageSubcommand MovePageSubcommand { get; } = new();

    [Subcommand(RemovePagesSubcommand.Name, RemovePagesSubcommand.Description)]
    public RemovePagesSubcommand RemovePageSubcommand { get; } = new();

    [Subcommand(RemovePasswordSubcommand.Name, RemovePasswordSubcommand.Description)]
    public RemovePasswordSubcommand RemovePasswordSubcommand { get; } = new();

    [Subcommand(RemoveWatermarkAnnotationSubcommand.Name, RemoveWatermarkAnnotationSubcommand.Description)]
    public RemoveWatermarkAnnotationSubcommand RemoveWatermarkAnnotationSubcommand { get; } = new();

    [Subcommand(ReorderPagesSubcommand.Name, ReorderPagesSubcommand.Description)]
    public ReorderPagesSubcommand ReorderPagesSubcommand { get; } = new();

    [Subcommand(RotatePagesSubcommand.Name, RotatePagesSubcommand.Description)]
    public RotatePagesSubcommand RotatePagesSubcommand { get; } = new();

    [Subcommand(SetPasswordSubcommand.Name, SetPasswordSubcommand.Description)]
    public SetPasswordSubcommand SetPasswordSubcommand { get; } = new();

    [Subcommand(SplitSubcommand.Name, SplitSubcommand.Description)]
    public SplitSubcommand SplitSubcommand { get; } = new();

    [Subcommand(TextToPdfSubcommand.Name, TextToPdfSubcommand.Description)]
    public TextToPdfSubcommand TextToPdfSubcommand { get; } = new();
}
