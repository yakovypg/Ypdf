namespace YpdfDesktop.Infrastructure.Behaviors.Text
{
    public class PositiveIntNumberBoxBehavior : IntNumberBoxBehavior
    {
        protected override bool IsPossibleTextValid(string? possibleText, string? currentText)
        {
            possibleText ??= string.Empty;       
            return int.TryParse(possibleText, out int value) && value > 0;
        }
    }
}
