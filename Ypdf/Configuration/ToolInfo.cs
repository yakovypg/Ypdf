using System.Reflection;

namespace Ypdf.Configuration
{
    internal static class ToolInfo
    {
        public const string PyImageExtractor = "ImageExtractor.py";

        public static string WorkingDirectory { get; }
        public static string TempDirectory { get; }

        static ToolInfo()
        {
            string? assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string? workingDirectory = Path.GetDirectoryName(assemblyLocation);

            WorkingDirectory = workingDirectory ?? string.Empty;
            TempDirectory = Path.Combine(WorkingDirectory, "temp");
        }

        public static void PrepareDirectories()
        {
            if (!Directory.Exists(TempDirectory))
                Directory.CreateDirectory(TempDirectory);
            else
                ClearTempFiles();
        }

        public static void ClearTempFiles()
        {
            if (!Directory.Exists(TempDirectory))
                return;

            Directory.Delete(TempDirectory, true);
            Directory.CreateDirectory(TempDirectory);
        }
    }
}
