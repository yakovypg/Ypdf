using System.Reflection;

namespace SharedConfig
{
    public static class Directories
    {
        public static readonly string AssemplyLocation = Assembly.GetEntryAssembly()?.Location ?? string.Empty;
        public static readonly string AssemplyDirectory = Path.GetDirectoryName(AssemplyLocation) ?? string.Empty;

        public static readonly string Temp = Path.Combine(AssemplyDirectory, "Temp");
        public static readonly string Docs = Path.Combine(AssemplyDirectory, "Docs");
        public static readonly string Fonts = Path.Combine(AssemplyDirectory, "Fonts");
        public static readonly string Config = Path.Combine(AssemplyDirectory, "Config");
        public static readonly string Scripts = Path.Combine(AssemplyDirectory, "Scripts");
        public static readonly string Locales = Path.Combine(AssemplyDirectory, "Locales");
        public static readonly string Themes = Path.Combine(AssemplyDirectory, "Themes");

        static Directories()
        {
            Prepare();
        }

        public static void Prepare()
        {
            PrepareDirectory(Temp);
            PrepareDirectory(Docs);
            PrepareDirectory(Fonts);
            PrepareDirectory(Config);
            PrepareDirectory(Scripts);
            PrepareDirectory(Locales);
            PrepareDirectory(Themes);
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
