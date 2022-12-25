namespace YpdfLib.Compressors
{
    public class CompressionException : ApplicationException
    {
        public CompressionException(string? message = null, Exception? innerException = null)
            : base(message ?? "Compression failed.", innerException)
        {
        }
    }
}
