namespace FileSystemLib.Paths
{
    public static class PathCorrector
    {
        public static IPathMulticorrection[] CorrectFilePaths(params string[] paths)
        {
            return CorrectFilePaths(Array.Empty<string>(), paths);
        }

        public static IPathMulticorrection[] CorrectFilePaths(string[] preferredExtensions, string[] paths)
        {
            if (paths is null)
                throw new ArgumentNullException(nameof(paths));

            var corrections = new IPathMulticorrection[paths.Length];

            for (int i = 0; i < paths.Length; ++i)
            {
                string path = paths[i];

                if (string.IsNullOrEmpty(path))
                {
                    corrections[i] = new PathMulticorrection(path, null);
                    continue;
                }
                else if (File.Exists(path))
                {
                    corrections[i] = new PathMulticorrection(path, null, path);
                    continue;
                }

                FileInfo? fileInfo = new FileInfo(path);
                DirectoryInfo? parentDir = Directory.GetParent(path);

                if (fileInfo is null || parentDir is null)
                {
                    corrections[i] = new PathMulticorrection(path, null);
                    continue;
                }

                string[] predictions = parentDir
                    .GetFiles($"{fileInfo.Name}.*")
                    .Select(t => t.FullName)
                    .ToArray();

                if (predictions.Length == 0)
                {
                    corrections[i] = new PathMulticorrection(path, null);
                    continue;
                }

                var correction = new PathMulticorrection(path, predictions);
                correction.Correct(preferredExtensions);

                corrections[i] = correction;
            }

            return corrections;
        }
    }
}
