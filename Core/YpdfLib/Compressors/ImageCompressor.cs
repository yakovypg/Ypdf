using RuntimeLib.Python;
using YpdfLib.Models.Imaging;

namespace YpdfLib.Compressors
{
    public static class ImageCompressor
    {
        private const bool CONVERT_COMMA_TO_DOT = true;

        public static void Compress(string inputFile, string destPath, string? pythonAlias = null,
            TextWriter? outputWriter = null)
        {
            Compress(inputFile, destPath, new ImageCompression(), pythonAlias, outputWriter);
        }

        public static void Compress(string inputFile, string destPath, IImageCompression compression,
            string? pythonAlias = null, TextWriter? outputWriter = null)
        {
            var executor = new PythonExecutor(true, true, outputWriter)
            {
                RequirePython3 = true
            };

            if (!string.IsNullOrEmpty(pythonAlias))
                executor.PythonAlias = pythonAlias;

            string sizeFactor = ConvertFloatToString(compression.SizeFactor);
            string qualityFactor = ConvertFloatToString(compression.QualityFactor);

            string pythonImageCompressorPath = SharedConfig.Scripts.PythonImageCompressor;
            string args = $"{pythonImageCompressorPath} -i {inputFile} -o {destPath} -q {qualityFactor} -s {sizeFactor}";

            if (compression.Width is not null)
                args += $" -W {compression.Width}";

            if (compression.Height is not null)
                args += $" -H {compression.Height}";

            executor.Execute(args);
        }

        public static void Compress(string[] inputFiles, string destDir, string? pythonAlias = null,
            TextWriter? outputWriter = null)
        {
            Compress(inputFiles, destDir, new ImageCompression(), pythonAlias, outputWriter);
        }

        public static void Compress(string[] inputFiles, string destDir, IImageCompression compression,
            string? pythonAlias = null, TextWriter? outputWriter = null)
        {
            if (inputFiles.Length == 0)
                return;

            var executor = new PythonExecutor(true, true, outputWriter)
            {
                RequirePython3 = true
            };

            if (!string.IsNullOrEmpty(pythonAlias))
                executor.PythonAlias = pythonAlias;

            if (string.IsNullOrEmpty(destDir))
                destDir = "\"\"";

            string pythonImageCompressorPath = SharedConfig.Scripts.PythonImageCompressor;

            string sizeFactor = ConvertFloatToString(compression.SizeFactor);
            string qualityFactor = ConvertFloatToString(compression.QualityFactor);

            string files = string.Join(' ', inputFiles);
            string args = $"{pythonImageCompressorPath} -i {files} -O {destDir} -q {qualityFactor} -s {sizeFactor}";

            if (!string.IsNullOrEmpty(compression.Extension))
                args += $" -e {compression.Extension}";

            executor.Execute(args);
        }

        private static string ConvertFloatToString(float value)
        {
            return CONVERT_COMMA_TO_DOT
                ? value.ToString().Replace(',', '.')
                : value.ToString();
        }
    }
}
