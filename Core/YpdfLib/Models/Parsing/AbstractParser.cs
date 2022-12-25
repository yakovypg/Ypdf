namespace YpdfLib.Models.Parsing
{
    public static class AbstractParser
    {
        public static T Parse<T>(string data, Func<string, T> parser)
        {
            return parser.Invoke(data);
        }

        public static T[] ParseMany<T>(string data, char delimiter, Func<string, T> parser)
        {
            return data.Split(delimiter)
                .Select(t => parser.Invoke(t))
                .ToArray();
        }

        public static T[] ParseManyToOne<T>(string data, char setDelimiter, char partDelimiter, Func<string, T> parser)
        {
            string[] parts = data.Split(partDelimiter);

            if (parts.Length != 2)
            {
                string message = $"Incorrect rotation foramt. Format: " +
                    $"x1{setDelimiter}...{setDelimiter}xn{partDelimiter}Y.";

                throw new ArgumentException(message, nameof(data));
            }

            string[] setItems = parts[0].Split(setDelimiter);

            return setItems
                .Select(t => $"{t}{partDelimiter}{parts[1]}")
                .Select(t => parser.Invoke(t))
                .ToArray();
        }
    }
}
