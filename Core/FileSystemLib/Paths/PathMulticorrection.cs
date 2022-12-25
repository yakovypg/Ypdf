namespace FileSystemLib.Paths
{
    public class PathMulticorrection : IPathMulticorrection
    {
        public string OldPath { get; }
        public string? NewPath { get; private set; }
        public string[] Corrections { get; }

        public bool IsPathCorrected => NewPath is not null;
        public bool IsRemainedUnchanged => OldPath == NewPath;

        public PathMulticorrection(string oldPath, string[]? corrections, string? newPath = null)
        {
            OldPath = oldPath;
            NewPath = newPath;
            Corrections = corrections ?? Array.Empty<string>();
        }

        public void Correct(string[] preferredExtensions)
        {
            if (preferredExtensions is null || preferredExtensions.Length == 0)
            {
                NewPath = Corrections.FirstOrDefault();
                return;
            }

            foreach (string extension in preferredExtensions)
            {
                string? path = Corrections.FirstOrDefault(path =>
                {
                    string? pathExtension = null;

                    try
                    {
                        pathExtension = Path.GetExtension(path);
                    }
                    catch { }

                    return pathExtension == extension;
                });

                if (!string.IsNullOrEmpty(path))
                {
                    NewPath = path;
                    return;
                }
            }

            NewPath = Corrections.FirstOrDefault();
        }

        public void Correct(string newPath)
        {
            if (!Corrections.Contains(newPath))
                throw new ArgumentException($"The path '{newPath}' is not included in the corrections.", nameof(newPath));

            NewPath = newPath;
        }

        public void Correct(int correctionIndex)
        {
            NewPath = Corrections[correctionIndex];
        }
    }
}
