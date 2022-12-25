namespace ypdf.Parsing
{
    internal class IncorrectParameterValueException : ApplicationException
    {
        public string Name { get; }
        public string Value { get; }

        public IncorrectParameterValueException(string name, string value, string? message = null, Exception? innerException = null)
            : base(message ?? $"Incorrect value for parameter '{name}': {value}", innerException)
        {
            Name = name;
            Value = value;
        }
    }
}
