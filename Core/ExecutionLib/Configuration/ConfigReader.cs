namespace ExecutionLib.Configuration
{
    public static class ConfigReader
    {
        public static string? ReadText(string path)
        {
            string text = File.ReadAllText(path);

            return string.IsNullOrEmpty(text)
                ? null
                : text;
        }

        public static string? TryReadText(string path)
        {
            try
            {
                return ReadText(path);
            }
            catch
            {
                return null;
            }
        }
    }
}
