using System.IO;

namespace Ypdf.Paths;

public static class Files
{
    static Files()
    {
        Readme = Path.Combine(Directories.Docs, "README.md");
        PythonAlias = Path.Combine(Directories.Config, "python-alias.txt");
        UIConfig = Path.Combine(Directories.Config, "ui-config.json");
    }

    public static string Readme { get; }
    public static string PythonAlias { get; }
    public static string UIConfig { get; }

    public static void Prepare()
    {
        PrepareFile(Readme);
        PrepareFile(PythonAlias);
        PrepareFile(UIConfig);
    }

    private static void PrepareFile(string path)
    {
        if (string.IsNullOrEmpty(path) || File.Exists(path))
            return;

        File.Create(path).Close();
    }
}
