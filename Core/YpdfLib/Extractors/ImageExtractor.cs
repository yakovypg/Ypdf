using RuntimeLib.Python;

namespace YpdfLib.Extractors
{
    public static class ImageExtractor
    {
        public static void Extract(string destDir, params string[] inputFiles)
        {
            if (inputFiles is null)
                throw new ArgumentNullException(nameof(inputFiles));

            if (inputFiles.Length == 0)
                return;

            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            string paths = string.Join(' ', inputFiles);
            string pythonImageExtractorPath = Properties.Resources.PYTHON_IMAGE_EXTRACTOR;

            var executor = new PythonExecutor(true, true, Console.Out)
            {
                ErrorDataVerifier = t => !string.IsNullOrEmpty(t),
                ErrorDataConverter = t => t != null && t.Contains("pages") ? $"\n{t}" : t
            };

            executor.Execute($"{pythonImageExtractorPath} -o {destDir} -i {paths}");
        }
    }
}
