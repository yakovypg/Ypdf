namespace SharedConfig
{
    public static class Files
    {
        public static readonly string Readme = Path.Combine(Directories.DOCS, "README.md");
        public static readonly string PythonAlias = Path.Combine(Directories.CONFIG, "python-alias.txt");

        static Files()
        {
            Prepare();
        }

        public static void Prepare()
        {
            PrepareFile(Readme);
            PrepareFile(PythonAlias);
        }

        private static void PrepareFile(string path)
        {
            if (string.IsNullOrEmpty(path) || File.Exists(path))
                return;

            try
            {
                File.Create(path).Close();
            }
            catch { }
        }
    }
}
