namespace Ypdf.Security
{
    public class PasswordList : IPasswordGenerator, IDisposable
    {
        private readonly StreamReader _passwordReader;

        public bool IsOver => _passwordReader.EndOfStream;

        public PasswordList(string passwordListPath)
        {
            if (!File.Exists(passwordListPath))
                throw new FileNotFoundException(passwordListPath);

            _passwordReader = new StreamReader(passwordListPath);
        }

        public string? Next()
        {
            return _passwordReader.ReadLine();
        }

        public void Dispose()
        {
            _passwordReader.Dispose();
        }
    }
}
