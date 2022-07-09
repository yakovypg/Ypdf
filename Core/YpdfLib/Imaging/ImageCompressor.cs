using RuntimeLib.Python;

namespace YpdfLib.Imaging
{
    public static class ImageCompressor
    {
        public static void Compress(string inputFile, string destPath, float qualityFactor = 0.75f, float sizeFactor = 1.0f, int? width = null, int? height = null)
        {
            var executor = new PythonExecutor(true, true, Console.Out);
            string pythonImageCompressorPath = Properties.Resources.PYTHON_IMAGE_COMPRESSOR;

            string args = $"{pythonImageCompressorPath} -i {inputFile} -o {destPath} -q {qualityFactor} -s {sizeFactor}";

            if (width is not null)
                args += $" -W {width}";

            if (height is not null)
                args += $" -H {height}";

            executor.Execute(args);
        }

        public static void Compress(string[] inputFiles, string destDir, float qualityFactor = 0.75f, float sizeFactor = 1.0f, string extension = "")
        {
            var executor = new PythonExecutor(true, true, Console.Out);
            string pythonImageCompressorPath = Properties.Resources.PYTHON_IMAGE_COMPRESSOR;

            string files = string.Join(' ', inputFiles);
            string args = $"{pythonImageCompressorPath} -i {files} -O {destDir} -q {qualityFactor} -s {sizeFactor}";

            if (!string.IsNullOrEmpty(extension) && !string.IsNullOrWhiteSpace(extension))
                args += $" -e {extension}";

            executor.Execute(args);
        }
    }
}
