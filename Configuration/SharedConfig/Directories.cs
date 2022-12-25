namespace SharedConfig
{
    public static class Directories
    {
        public const string TEMP = "Temp";
        public const string DOCS = "Docs";
        public const string FONTS = "Fonts";
        public const string CONFIG = "Config";
        public const string SCRIPTS = "Scripts";

        static Directories()
        {
            Prepare();
        }

        public static void Prepare()
        {
            PrepareDirectory(TEMP);
            PrepareDirectory(DOCS);
            PrepareDirectory(FONTS);
            PrepareDirectory(CONFIG);
            PrepareDirectory(SCRIPTS);
        }

        private static void PrepareDirectory(string path)
        {
            if (string.IsNullOrEmpty(path) || Directory.Exists(path))
                return;

            try
            {
                Directory.CreateDirectory(path);
            }
            catch { }
        }
    }
}
