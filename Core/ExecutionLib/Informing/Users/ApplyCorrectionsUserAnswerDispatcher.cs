using FileSystemLib.Paths;

namespace ExecutionLib.Informing.Users
{
    public class ApplyCorrectionsUserAnswerDispatcher : IApplyCorrectionsUserAnswerDispatcher, IEquatable<ApplyCorrectionsUserAnswerDispatcher?>
    {
        private readonly Func<IPathCorrection[], UserAnswer> _questionFunc;

        public ApplyCorrectionsUserAnswerDispatcher(Func<IPathCorrection[], UserAnswer> questionFunc)
        {
            _questionFunc = questionFunc;
        }

        public UserAnswer Ask(IPathCorrection[] corrections)
        {
            return _questionFunc?.Invoke(corrections) ?? UserAnswer.Unknown;
        }

        public static Func<IPathCorrection[], UserAnswer> GetConsoleQuestionFunc()
        {
            return corrections =>
            {
                string? s = corrections.Length > 1 ? "s" : null;

                Console.WriteLine($"Suggested correction{s}:");

                foreach (var correction in corrections)
                    Console.WriteLine($"{correction.OldPath} -> {correction.NewPath}");

                Console.WriteLine();
                Console.Write($"Apply correction{s} (y/n): ");

                string? answer = Console.ReadLine()?.ToLower();
                bool replaceFile = answer == "y" || answer == "yes";

                return replaceFile ? UserAnswer.Yes : UserAnswer.No;
            };
        }

        public bool Equals(ApplyCorrectionsUserAnswerDispatcher? other)
        {
            return other is not null &&
                   EqualityComparer<Func<IPathCorrection[], UserAnswer>>.Default.Equals(_questionFunc, other._questionFunc);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ApplyCorrectionsUserAnswerDispatcher);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_questionFunc);
        }
    }
}
