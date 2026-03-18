using System.IO;

namespace Ypdf.Paths;

public static class Files
{
    private const string EmptyJson = "{}";

    static Files()
    {
        Config = Path.Combine(Directories.Config, "config.json");
    }

    public static string Config { get; }

    public static void Prepare()
    {
        PrepareFile(Config, EmptyJson);
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
