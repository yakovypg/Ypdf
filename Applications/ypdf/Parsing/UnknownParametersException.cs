namespace ypdf.Parsing
{
    internal class UnknownParametersException : ApplicationException
    {
        public string[] UnknownParameters { get; }

        public UnknownParametersException(string[] unknownParameters, string? message = null, Exception? innerException = null)
            : base(message ?? GetDefaultMessage(unknownParameters), innerException)
        {
            UnknownParameters = unknownParameters;
        }

        private static string GetDefaultMessage(string[] unknownParameters)
        {
            if (unknownParameters.Length == 0)
                return "Parameters are unknown.";

            if (unknownParameters.Length == 1)
                return $"Parameter {unknownParameters[0]} is unknown.";

            string parameters = string.Join(", ", unknownParameters);
            string message = $"Parameters {parameters} are unknown.";

            return message;
        }
    }
}
