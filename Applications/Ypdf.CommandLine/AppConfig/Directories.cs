using System.IO;
using System.Reflection;
using Ypdf.Core.Config;

namespace Ypdf.CommandLine.AppConfig;

internal static class Directories
{
    static Directories()
    {
        AssemblyLocation = Assembly.GetEntryAssembly()?.Location ?? string.Empty;
        RootDirectory = Path.GetDirectoryName(AssemblyLocation) ?? string.Empty;

        Config = Path.Combine(RootDirectory, "Config");
    }

    internal static string AssemblyLocation { get; }
    internal static string RootDirectory { get; }

    internal static string Config { get; }

    internal static void Prepare()
    {
        PrepareDirectory(Config);
        CoreDirectories.Prepare();
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

    private static void PrepareDirectory(string path)
    {
        if (string.IsNullOrEmpty(path) || Directory.Exists(path))
            return;

        Directory.CreateDirectory(path);
    }
}
