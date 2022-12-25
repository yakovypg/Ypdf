namespace ExecutionLib.Configuration
{
    public static class ConfigWriter
    {
        public static void WriteText(string path, string? text)
        {
            File.WriteAllText(path, text ?? string.Empty);
        }

        public static bool TryWriteText(string path, string? text)
        {
            try
            {
                WriteText(path, text);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
