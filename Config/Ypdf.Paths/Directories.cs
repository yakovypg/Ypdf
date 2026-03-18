using System.IO;
using System.Reflection;

namespace Ypdf.Paths;

public static class Directories
{
    static Directories()
    {
        AssemblyLocation = Assembly.GetEntryAssembly()?.Location ?? string.Empty;
        RootDirectory = Path.GetDirectoryName(AssemblyLocation) ?? string.Empty;

        Temp = Path.Combine(RootDirectory, "Temp");
        Docs = Path.Combine(RootDirectory, "Docs");
        Fonts = Path.Combine(RootDirectory, "Fonts");
        Config = Path.Combine(RootDirectory, "Config");
        Scripts = Path.Combine(RootDirectory, "Scripts");
        Locales = Path.Combine(RootDirectory, "Locales");
        Themes = Path.Combine(RootDirectory, "Themes");
    }

    public static string AssemblyLocation { get; }
    public static string RootDirectory { get; }

    public static string Temp { get; }
    public static string Docs { get; }
    public static string Fonts { get; }
    public static string Config { get; }
    public static string Scripts { get; }
    public static string Locales { get; }
    public static string Themes { get; }

    public static void Prepare()
    {
        PrepareDirectory(Temp);
        PrepareDirectory(Docs);
        PrepareDirectory(Fonts);
        PrepareDirectory(Config);
        PrepareDirectory(Scripts);
        PrepareDirectory(Locales);
        PrepareDirectory(Themes);
    }

    private static void PrepareDirectory(string path)
    {
        if (string.IsNullOrEmpty(path) || Directory.Exists(path))
            return;

        Directory.CreateDirectory(path);
    }
}
