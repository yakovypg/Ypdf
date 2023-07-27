using Avalonia;

namespace YpdfDesktop.Infrastructure.Behaviors.Text
{
    public abstract class TextValidatorBehavior<T> : TextBehavior<T> where T : class, IAvaloniaObject
    {
        protected virtual bool IsPossibleTextValid(string? possibleText, string? currentText)
        {
            return true;
        }
    }
}
