namespace ExecutionLib.Informing.Users
{
    public class FileExistsUserAnswerDispatcher : IFileExistsUserAnswerDispatcher, IEquatable<FileExistsUserAnswerDispatcher?>
    {
        private readonly Func<string, UserAnswer> _questionFunc;

        public FileExistsUserAnswerDispatcher(Func<string, UserAnswer> questionFunc)
        {
            _questionFunc = questionFunc;
        }

        public UserAnswer Ask(string filePath)
        {
            return _questionFunc?.Invoke(filePath) ?? UserAnswer.Unknown;
        }

        public static Func<string, UserAnswer> GetConsoleQuestionFunc()
        {
            return path =>
            {
                Console.Write($"File {path} already exists. Replace it (y/n): ");

                string? answer = Console.ReadLine()?.ToLower();
                bool replaceFile = answer == "y" || answer == "yes";

                return replaceFile ? UserAnswer.Yes : UserAnswer.No;
            };
        }

        public bool Equals(FileExistsUserAnswerDispatcher? other)
        {
            return other is not null &&
                   EqualityComparer<Func<string, UserAnswer>>.Default.Equals(_questionFunc, other._questionFunc);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as FileExistsUserAnswerDispatcher);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_questionFunc);
        }
    }
}
