namespace YpdfLib.Models.Imaging
{
    public interface IImageCompression : IDeepCloneable<IImageCompression>
    {
        float QualityFactor { get; set; }
        float SizeFactor { get; set; }

        int? Width { get; set; }
        int? Height { get; set; }

        string? Extension { get; set; }
        bool CheckCompressionValidity { get; set; }
    }
}
