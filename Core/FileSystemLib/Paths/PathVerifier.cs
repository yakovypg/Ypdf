namespace FileSystemLib.Paths
{
    public static class PathVerifier
    {
        public static bool VerifyFileExists(out List<string> unverifiedFiles, params string[] paths)
        {
            return VerifyPaths(File.Exists, out unverifiedFiles, paths);
        }

        public static bool VerifyDirectoryExists(out List<string> unverifiedDirectories, params string[] paths)
        {
            return VerifyPaths(Directory.Exists, out unverifiedDirectories, paths);
        }

        public static bool VerifyPaths(Predicate<string> verifier, out List<string> unverifiedPaths, params string[] paths)
        {
            if (paths is null)
                throw new ArgumentNullException(nameof(paths));

            unverifiedPaths = new List<string>();

            foreach (string path in paths)
            {
                if (!verifier.Invoke(path))
                    unverifiedPaths.Add(path);
            }

            return unverifiedPaths.Count == 0;
        }
    }
}
