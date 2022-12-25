using RuntimeLib.Python;

namespace YpdfLib.Extractors
{
    public static class ImageExtractor
    {
        public static void Extract(string destDir, string[] inputFiles, int extractedImagesLimit = 0, TextWriter? outputWriter = null)
        {
            Extract(destDir, null, inputFiles, extractedImagesLimit, outputWriter);
        }

        public static void Extract(string destDir, string? pythonAlias, string[] inputFiles, int extractedImagesLimit = -1, TextWriter? outputWriter = null)
        {
            if (inputFiles is null)
                throw new ArgumentNullException(nameof(inputFiles));

            if (inputFiles.Length == 0)
                return;

            if (string.IsNullOrEmpty(destDir))
                destDir = "\"\"";

            string paths = string.Join(' ', inputFiles);
            string pythonImageExtractorPath = SharedConfig.Scripts.PythonImageExtractor;

            var executor = new PythonExecutor(true, true, outputWriter)
            {
                RequirePython3 = true,
                ErrorDataVerifier = t => !string.IsNullOrEmpty(t),
                ErrorDataConverter = t => t != null && t.Contains("pages") ? $"\n{t}" : t
            };

            if (!string.IsNullOrEmpty(pythonAlias))
                executor.PythonAlias = pythonAlias;

            executor.Execute($"{pythonImageExtractorPath} -l {extractedImagesLimit} -o {destDir} -i {paths}");
        }
    }
}
