using System.IO;
using System.Reflection;

namespace Ypdf.Core.Config;

public static class CoreDirectories
{
    static CoreDirectories()
    {
        AssemblyLocation = Assembly.GetEntryAssembly()?.Location ?? string.Empty;
        RootDirectory = Path.GetDirectoryName(AssemblyLocation) ?? string.Empty;

        Temp = Path.Combine(RootDirectory, "Temp");
        Fonts = Path.Combine(RootDirectory, "Fonts");
        Scripts = Path.Combine(RootDirectory, "Scripts");
    }

    public static string AssemblyLocation { get; }
    public static string RootDirectory { get; }

    public static string Temp { get; }
    public static string Fonts { get; }
    public static string Scripts { get; }

    public static void Prepare()
    {
        PrepareDirectory(Temp);
        PrepareDirectory(Fonts);
        PrepareDirectory(Scripts);
    }

    private static void PrepareDirectory(string path)
    {
        if (string.IsNullOrEmpty(path) || Directory.Exists(path))
            return;

        Directory.CreateDirectory(path);
    }
}
