namespace ExecutionLib.Informing.Users
{
    public class UserAnswerDispatcher : IUserAnswerDispatcher, IEquatable<UserAnswerDispatcher?>
    {
        private readonly Func<UserAnswer> _questionFunc;

        public UserAnswerDispatcher(Func<UserAnswer> questionFunc)
        {
            _questionFunc = questionFunc;
        }

        public UserAnswer Ask()
        {
            return _questionFunc?.Invoke() ?? UserAnswer.Unknown;
        }

        public bool Equals(UserAnswerDispatcher? other)
        {
            return other is not null &&
                   EqualityComparer<Func<UserAnswer>>.Default.Equals(_questionFunc, other._questionFunc);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UserAnswerDispatcher);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_questionFunc);
        }
    }
}
