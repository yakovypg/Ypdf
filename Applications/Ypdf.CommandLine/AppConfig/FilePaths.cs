using System.IO;

namespace Ypdf.CommandLine.AppConfig;

internal static class FilePaths
{
    private const string _emptyJson = "{}";
    private const string _configFileName = "config.json";

    static FilePaths()
    {
        Config = GetConfigFilePath();
    }

    internal static string Config { get; }

    internal static void Prepare()
    {
        PrepareFile(Config, _emptyJson);
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

    private static string GetConfigFilePath()
    {
        try
        {
            return Path.Combine(Directories.Config, _configFileName);
        }
        catch
        {
            return $"{Directories.Config}/{_configFileName}";
        }
    }
}
