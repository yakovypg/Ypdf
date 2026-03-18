using System.IO;

namespace Ypdf.Core.Config;

public static class PythonScriptPaths
{
    static PythonScriptPaths()
    {
        TextExtractor = Path.Combine(CoreDirectories.Scripts, "TextExtractor.py");
        ImageExtractor = Path.Combine(CoreDirectories.Scripts, "ImageExtractor.py");
        ImageCompressor = Path.Combine(CoreDirectories.Scripts, "ImageCompressor.py");
    }

    public static string TextExtractor { get; }
    public static string ImageExtractor { get; }
    public static string ImageCompressor { get; }
}
