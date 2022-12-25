namespace YpdfLib.Models.Imaging
{
    public class ImageCompression : IImageCompression, IDeepCloneable<ImageCompression>, IEquatable<ImageCompression?>
    {
        public float QualityFactor { get; set; }
        public float SizeFactor { get; set; }

        public int? Width { get; set; }
        public int? Height { get; set; }

        public string? Extension { get; set; }
        public bool CheckCompressionValidity { get; set; }

        public ImageCompression() : this(0.75f, 1.0f, null, null, null, true)
        {
        }

        public ImageCompression(float qualityFactor, float sizeFactor, string? extension, bool checkCompressionValidity = true)
            : this(qualityFactor, sizeFactor, null, null, extension, checkCompressionValidity)
        {
        }

        public ImageCompression(float qualityFactor, float sizeFactor, int? width, int? height, bool checkCompressionValidity = true)
            : this(qualityFactor, sizeFactor, width, height, null, checkCompressionValidity)
        {
        }

        public ImageCompression(float qualityFactor, float sizeFactor, int? width, int? height, string? extension,
            bool checkCompressionValidity = true)
        {
            QualityFactor = qualityFactor;
            SizeFactor = sizeFactor;
            Width = width;
            Height = height;
            Extension = extension;
            CheckCompressionValidity = checkCompressionValidity;
        }

        public ImageCompression Copy()
        {
            return new ImageCompression(QualityFactor, SizeFactor, Width, Height, Extension, CheckCompressionValidity);
        }

        IImageCompression IDeepCloneable<IImageCompression>.Copy()
        {
            return Copy();
        }

        public bool Equals(ImageCompression? other)
        {
            return other is not null &&
                   QualityFactor == other.QualityFactor &&
                   SizeFactor == other.SizeFactor &&
                   Width == other.Width &&
                   Height == other.Height &&
                   Extension == other.Extension &&
                   CheckCompressionValidity == other.CheckCompressionValidity;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ImageCompression);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(QualityFactor, SizeFactor, Width, Height, Extension, CheckCompressionValidity);
        }
    }
}
