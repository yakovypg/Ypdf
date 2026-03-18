using System.IO;

namespace Ypdf.CommandLine.AppConfig;

internal static class FilePaths
{
    private const string EmptyJson = "{}";

    static FilePaths()
    {
        Config = Path.Combine(Directories.Config, "config.json");
    }

    internal static string Config { get; }

    internal static void Prepare()
    {
        PrepareFile(Config, EmptyJson);
    }

    internal static bool TryPrepare()
    {
        try
        {
            Prepare();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static void PrepareFile(string path, string? content = null)
    {
        if (string.IsNullOrEmpty(path) || File.Exists(path))
            return;

        if (string.IsNullOrEmpty(content))
            File.Create(path).Close();
        else
            File.WriteAllText(path, content);
    }
}
