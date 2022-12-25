namespace FileSystemLib.Paths
{
    public static class DirectoryPreparer
    {
        public static void PrepareOutputDirectories(params string[] paths)
        {
            if (paths is null)
                throw new ArgumentNullException(nameof(paths));

            foreach (string path in paths)
            {
                if (string.IsNullOrEmpty(path))
                    continue;

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }

        public static void PrepareFileDirectories(params string[] paths)
        {
            if (paths is null)
                throw new ArgumentNullException(nameof(paths));

            foreach (string path in paths)
            {
                if (string.IsNullOrEmpty(path))
                    continue;

                DirectoryInfo? parentDir = Directory.GetParent(path);

                if (parentDir is null)
                    throw new IncorrectPathException(path);

                if (!Directory.Exists(parentDir.FullName))
                    Directory.CreateDirectory(parentDir.FullName);
            }
        }
    }
}
