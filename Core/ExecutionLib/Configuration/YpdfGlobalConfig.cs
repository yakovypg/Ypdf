using YpdfLib.Models;

namespace ExecutionLib.Configuration
{
    public class YpdfGlobalConfig : IYpdfGlobalConfig, IDeepCloneable<YpdfGlobalConfig>, IEquatable<YpdfGlobalConfig?>
    {
        public string? PythonAlias { get; set; }

        public YpdfGlobalConfig()
        {
            LoadConfig();
        }

        public YpdfGlobalConfig Copy()
        {
            return new YpdfGlobalConfig()
            {
                PythonAlias = PythonAlias
            };
        }

        IYpdfGlobalConfig IDeepCloneable<IYpdfGlobalConfig>.Copy()
        {
            return Copy();
        }

        public string[] GetItems()
        {
            return GetType()
                .GetProperties()
                .Select(t => $"{t.Name}: {t.GetValue(this) ?? "null"}")
                .ToArray();
        }

        public void Reset(bool saveChanges = false)
        {
            PythonAlias = null;

            if (saveChanges)
                Save();
        }

        public void Save()
        {
            ConfigWriter.WriteText(SharedConfig.Files.PythonAlias, PythonAlias);
        }

        private void LoadConfig()
        {
            PythonAlias = ConfigReader.TryReadText(SharedConfig.Files.PythonAlias);
        }

        public bool Equals(YpdfGlobalConfig? other)
        {
            return other is not null &&
                   PythonAlias == other.PythonAlias;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as YpdfGlobalConfig);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PythonAlias);
        }
    }
}
