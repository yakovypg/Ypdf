using Ypdf.Configuration;
using Ypdf.Runtime.Python;

namespace Ypdf.Converters
{
    public static class ImageExtractor
    {
        public static void Extract(string destDir, params string[] inputPaths)
        {
            if (inputPaths is null)
                throw new ArgumentNullException(nameof(inputPaths));

            if (inputPaths.Length == 0)
                return;

            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            string paths = string.Join(' ', inputPaths);

            var executor = new PythonExecutor(true, true, Console.Out)
            {
                ErrorDataVerifier = t => !string.IsNullOrEmpty(t),
                ErrorDataConverter = t => t != null && t.Contains("pages") ? $"\n{t}" : t
            };

            executor.Execute($"{ToolInfo.PyImageExtractor} {destDir} {paths}");
        }
    }
}
