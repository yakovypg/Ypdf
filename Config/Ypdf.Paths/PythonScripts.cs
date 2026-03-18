using System.IO;

namespace Ypdf.Paths;

public static class PythonScripts
{
    static PythonScripts()
    {
        TextExtractor = Path.Combine(Directories.Scripts, "TextExtractor.py");
        ImageExtractor = Path.Combine(Directories.Scripts, "ImageExtractor.py");
        ImageCompressor = Path.Combine(Directories.Scripts, "ImageCompressor.py");
    }

    public static string TextExtractor { get; }
    public static string ImageExtractor { get; }
    public static string ImageCompressor { get; }
}
