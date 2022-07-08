namespace YpdfLib.Security
{
    public class PasswordList : IPasswordGenerator, IDisposable
    {
        private readonly StreamReader _passwordReader;

        public bool IsOver => _passwordReader.EndOfStream;

        public PasswordList(string passwordListPath)
        {
            if (!File.Exists(passwordListPath))
                throw new FileNotFoundException("Password list file not found.", passwordListPath);

            _passwordReader = new StreamReader(passwordListPath);
        }

        public string? Next()
        {
            return !IsOver
                ? _passwordReader.ReadLine()
                : null;
        }

        public void Dispose()
        {
            _passwordReader.Dispose();
        }
    }
}
